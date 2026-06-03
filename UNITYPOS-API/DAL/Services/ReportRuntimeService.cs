using System.Data;
using System.Globalization;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Reports;

namespace UNITYPOS_API.DAL.Services
{
    public class ReportRuntimeService : IReportRuntimeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<ReportRuntimeService> _logger;

        public ReportRuntimeService(IUnitOfWork uow, IConfiguration configuration, IHostEnvironment environment, ILogger<ReportRuntimeService> logger)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<object> ExecuteAsync(ReportExecutionRequest request)
        {
            if (request.ReportId <= 0)
            {
                throw new ArgumentException("Valid ReportId is required.");
            }

            if (request.OrgId <= 0 || request.RoleId <= 0)
            {
                throw new ArgumentException("OrgId and RoleId are required.");
            }

            var report = (from reportMaster in _uow.GenericRepository<ReportMaster>().Table()
                          join permission in _uow.GenericRepository<ReportPermission>().Table()
                              on reportMaster.Id equals permission.ReportId
                          where reportMaster.Id == request.ReportId
                                && reportMaster.OrgId == request.OrgId
                                && permission.OrgId == request.OrgId
                                && permission.RoleId == request.RoleId
                                && permission.CanView == true
                                && reportMaster.IsDeleted == false
                                && reportMaster.IsActive == true
                          select new
                          {
                              reportMaster.Id,
                              reportMaster.ReportCode,
                              reportMaster.DisplayName,
                              reportMaster.Description,
                              reportMaster.StoredProcedure,
                              reportMaster.TemplateName,
                              reportMaster.TemplatePath,
                              reportMaster.ViewerType,
                              reportMaster.PaperWidth,
                              reportMaster.PaperHeight,
                              reportMaster.Orientation,
                              reportMaster.IsThermal,
                              reportMaster.IsLandscape,
                              permission.CanView,
                              permission.CanPrint,
                              permission.ExportPdf,
                              permission.ExportExcel
                          }).FirstOrDefault();

            if (report == null)
            {
                throw new KeyNotFoundException("Report definition not found or access is denied.");
            }

            var filters = _uow.GenericRepository<ReportFilter>().Table()
                .Where(filter => filter.ReportId == request.ReportId && filter.IsActive == true)
                .OrderBy(filter => filter.DisplayOrder)
                .ToList();

            var filterValues = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
            if (request.Filters != null)
            {
                foreach (var item in request.Filters)
                {
                    filterValues[item.Key] = item.Value;
                }
            }

            foreach (var filter in filters.Where(item => item.IsRequired))
            {
                if (!filterValues.TryGetValue(filter.FieldName, out var filterValue) || IsEmptyJsonValue(filterValue))
                {
                    throw new ArgumentException($"{filter.DisplayName} is required.");
                }
            }

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection is not configured.");
            }

            var rows = new List<Dictionary<string, object?>>();
            var columns = new List<object>();

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(report.StoredProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 180
            };

            SqlCommandBuilder.DeriveParameters(command);

            foreach (SqlParameter parameter in command.Parameters)
            {
                if (parameter.Direction == ParameterDirection.ReturnValue)
                {
                    continue;
                }

                var parameterKey = parameter.ParameterName.TrimStart('@');
                object parameterValue = DBNull.Value;

                if (filterValues.TryGetValue(parameterKey, out var jsonValue) && !IsEmptyJsonValue(jsonValue))
                {
                    parameterValue = ConvertJsonValueForParameter(parameterKey, jsonValue, parameter.SqlDbType);
                }

                parameter.Value = parameterValue ?? DBNull.Value;
            }

            try
            {
                // Log parameter snapshot right before execution to help diagnose missing/incorrect params
                try
                {
                    var paramInfo = string.Join(", ", command.Parameters.Cast<Microsoft.Data.SqlClient.SqlParameter>()
                        .Select(p => $"{p.ParameterName}={(p.Value == DBNull.Value ? "NULL" : p.Value)}:{p.SqlDbType}"));
                    _logger.LogInformation("Executing {Proc} with params: {Params}", command.CommandText, paramInfo);
                }
                catch (Exception logEx)
                {
                    _logger.LogDebug(logEx, "Failed to build parameter log for {Proc}", command.CommandText);
                }

                using var reader = await command.ExecuteReaderAsync();

                for (int index = 0; index < reader.FieldCount; index++)
                {
                    columns.Add(new
                    {
                        field = reader.GetName(index),
                        header = ToHeaderText(reader.GetName(index)),
                        type = ResolveColumnType(reader.GetFieldType(index))
                    });
                }

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        var value = await reader.IsDBNullAsync(index) ? null : reader.GetValue(index);
                        row[reader.GetName(index)] = value;
                    }

                    rows.Add(row);
                }
            }
            catch (SqlException ex)
            {
                var parameterSnapshot = BuildParameterSnapshot(filters, filterValues);
                _logger.LogError(ex,
                    "SQL execution failed for report {ReportCode} ({ReportId}) using stored procedure {StoredProcedure}. Parameters: {Parameters}",
                    report.ReportCode,
                    report.Id,
                    report.StoredProcedure,
                    parameterSnapshot);
                throw new InvalidOperationException(
                    $"SQL execution failed for ReportCode '{report.ReportCode}' using stored procedure '{report.StoredProcedure}'. Parameters: {parameterSnapshot}",
                    ex);
            }
            catch (Exception ex)
            {
                var parameterSnapshot = BuildParameterSnapshot(filters, filterValues);
                _logger.LogError(ex,
                    "Unexpected error while reading report data for report {ReportCode} ({ReportId}). Parameters: {Parameters}",
                    report.ReportCode,
                    report.Id,
                    parameterSnapshot);
                throw new InvalidOperationException(
                    $"Unexpected error while reading report data for ReportCode '{report.ReportCode}'. Parameters: {parameterSnapshot}",
                    ex);
            }

            return new
            {
                Report = new
                {
                    report.Id,
                    report.ReportCode,
                    report.DisplayName,
                    report.Description,
                    report.StoredProcedure,
                    report.TemplateName,
                    report.TemplatePath,
                    report.ViewerType,
                    report.PaperWidth,
                    report.PaperHeight,
                    report.Orientation,
                    report.IsThermal,
                    report.IsLandscape,
                    report.CanView,
                    report.CanPrint,
                    report.ExportPdf,
                    report.ExportExcel,
                    TemplateExists = ReportLayoutAssetHelper.Exists(_environment, _configuration, report.TemplatePath)
                },
                TemplateKind = ReportLayoutAssetHelper.ResolveTemplateKind(_environment, _configuration, report.TemplatePath),
                LayoutAsset = ReportLayoutAssetHelper.LoadJson(_environment, _configuration, report.TemplatePath),
                HtmlTemplate = ReportLayoutAssetHelper.LoadHtml(_environment, _configuration, report.TemplatePath),
                Columns = columns,
                Rows = rows,
                RowCount = rows.Count,
                ExecutedAt = DateTime.UtcNow
            };
        }

        private static bool IsEmptyJsonValue(JsonElement value)
        {
            return value.ValueKind == JsonValueKind.Null
                || value.ValueKind == JsonValueKind.Undefined
                || (value.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(value.GetString()));
        }

        private static string BuildParameterSnapshot(IEnumerable<ReportFilter> filters, IDictionary<string, JsonElement> filterValues)
        {
            return string.Join(", ", filters.Select(filter =>
            {
                var parameterName = filter.FieldName.StartsWith("@", StringComparison.Ordinal)
                    ? filter.FieldName
                    : $"@{filter.FieldName}";

                if (!filterValues.TryGetValue(filter.FieldName, out var value) || IsEmptyJsonValue(value))
                {
                    return $"{parameterName}=NULL";
                }

                return $"{parameterName}={FormatJsonValueForDebug(value)}";
            }));
        }

        private static string FormatJsonValueForDebug(JsonElement value)
        {
            return value.ValueKind switch
            {
                JsonValueKind.String => value.GetString() ?? string.Empty,
                JsonValueKind.Number => value.ToString(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                JsonValueKind.Null => "NULL",
                JsonValueKind.Undefined => "NULL",
                _ => value.ToString()
            };
        }

        private static object ConvertJsonValue(JsonElement value, string? dataType)
        {
            var normalizedDataType = string.Concat((dataType ?? string.Empty).Where(character => !char.IsWhiteSpace(character))).ToLowerInvariant();

            return normalizedDataType switch
            {
                "int" or "integer" => value.ValueKind == JsonValueKind.Number ? value.GetInt32() : int.Parse(value.GetString() ?? "0", CultureInfo.InvariantCulture),
                "bigint" => value.ValueKind == JsonValueKind.Number ? value.GetInt64() : long.Parse(value.GetString() ?? "0", CultureInfo.InvariantCulture),
                "decimal" or "numeric" or "money" => value.ValueKind == JsonValueKind.Number ? value.GetDecimal() : decimal.Parse(value.GetString() ?? "0", CultureInfo.InvariantCulture),
                "bit" or "bool" or "boolean" => value.ValueKind == JsonValueKind.True || (value.ValueKind == JsonValueKind.String && bool.Parse(value.GetString() ?? "false")),
                "date" or "datetime" or "datetime2" => value.ValueKind == JsonValueKind.String
                    ? DateTime.Parse(value.GetString() ?? string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
                    : value.GetDateTime(),
                _ => value.ValueKind == JsonValueKind.String ? value.GetString() ?? string.Empty : value.ToString()
            };
        }

        private static object ConvertJsonValueForParameter(string parameterName, JsonElement value, SqlDbType sqlDbType)
        {
            if (sqlDbType == SqlDbType.Date || sqlDbType == SqlDbType.DateTime || sqlDbType == SqlDbType.DateTime2)
            {
                var parsedDate = value.ValueKind == JsonValueKind.String
                    ? DateTime.Parse(value.GetString() ?? string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
                    : value.GetDateTime();

                if (parameterName.Equals("ToDate", StringComparison.OrdinalIgnoreCase)
                    && parsedDate.TimeOfDay == TimeSpan.Zero)
                {
                    return parsedDate.AddDays(1);
                }

                return parsedDate;
            }

            var dataType = sqlDbType switch
            {
                SqlDbType.Int => "int",
                SqlDbType.BigInt => "bigint",
                SqlDbType.Decimal => "decimal",
                SqlDbType.Money => "money",
                SqlDbType.Bit => "boolean",
                _ => "string"
            };

            return ConvertJsonValue(value, dataType);
        }

        private static string ResolveColumnType(Type fieldType)
        {
            if (fieldType == typeof(DateTime) || fieldType == typeof(DateTimeOffset))
            {
                return "date";
            }

            if (fieldType == typeof(decimal) || fieldType == typeof(double) || fieldType == typeof(float))
            {
                return "number";
            }

            if (fieldType == typeof(int) || fieldType == typeof(long) || fieldType == typeof(short))
            {
                return "number";
            }

            if (fieldType == typeof(bool))
            {
                return "boolean";
            }

            return "text";
        }

        private static string ToHeaderText(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                return string.Empty;
            }

            var buffer = new List<char>();

            for (int index = 0; index < fieldName.Length; index++)
            {
                var current = fieldName[index];

                if (index > 0 && char.IsUpper(current) && !char.IsWhiteSpace(fieldName[index - 1]))
                {
                    buffer.Add(' ');
                }

                buffer.Add(current == '_' ? ' ' : current);
            }

            return new string(buffer.ToArray()).Trim();
        }

    }
}
