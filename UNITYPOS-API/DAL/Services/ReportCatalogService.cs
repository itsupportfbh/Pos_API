using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Reports;

namespace UNITYPOS_API.DAL.Services
{
    public class ReportCatalogService : IReportCatalogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public ReportCatalogService(IUnitOfWork uow, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnumerable<object> GetCategories(int orgId, int roleId)
        {
            var result = (from category in _uow.GenericRepository<ReportCategory>().Table()
                          join report in _uow.GenericRepository<ReportMaster>().Table()
                              on category.Id equals report.CategoryId
                          join permission in _uow.GenericRepository<ReportPermission>().Table()
                              on report.Id equals permission.ReportId
                          where category.OrgId == orgId
                                && report.OrgId == orgId
                                && permission.OrgId == orgId
                                && permission.RoleId == roleId
                                && permission.CanView == true
                                && category.IsDeleted == false
                                && category.IsActive == true
                                && report.IsDeleted == false
                                && report.IsActive == true
                          group category by new
                          {
                              category.Id,
                              category.Name,
                              category.DisplayOrder
                          }
                into grouped
                          orderby grouped.Key.DisplayOrder, grouped.Key.Name
                          select new
                          {
                              grouped.Key.Id,
                              grouped.Key.Name,
                              grouped.Key.DisplayOrder,
                              ReportCount = grouped.Select(item => item.Id).Distinct().Count()
                          }).ToList<object>();

            return result;
        }

        public IEnumerable<object> GetReports(int orgId, int roleId, int? categoryId)
        {
            var reports = from report in _uow.GenericRepository<ReportMaster>().Table()
                          join category in _uow.GenericRepository<ReportCategory>().Table()
                              on report.CategoryId equals category.Id
                          join permission in _uow.GenericRepository<ReportPermission>().Table()
                              on report.Id equals permission.ReportId
                          where report.OrgId == orgId
                                && category.OrgId == orgId
                                && permission.OrgId == orgId
                                && permission.RoleId == roleId
                                && permission.CanView == true
                                && report.IsDeleted == false
                                && report.IsActive == true
                                && category.IsDeleted == false
                                && category.IsActive == true
                          select new
                          {
                              report.Id,
                              report.CategoryId,
                              CategoryName = category.Name,
                              report.ReportCode,
                              report.ReportName,
                              report.DisplayName,
                              report.Description,
                              report.StoredProcedure,
                              report.TemplateName,
                              report.TemplatePath,
                              report.ReportType,
                              report.ViewerType,
                              report.PaperWidth,
                              report.PaperHeight,
                              report.Orientation,
                              report.IsThermal,
                              report.IsLandscape,
                              report.DisplayOrder,
                              report.Icon,
                              permission.CanView,
                              permission.CanPrint,
                              permission.ExportPdf,
                              permission.ExportExcel
                          };

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                reports = reports.Where(item => item.CategoryId == categoryId.Value);
            }

            return reports
                .AsEnumerable()
                .Select(item => new
                {
                    item.Id,
                    item.CategoryId,
                    item.CategoryName,
                    item.ReportCode,
                    item.ReportName,
                    item.DisplayName,
                    item.Description,
                    item.StoredProcedure,
                    item.TemplateName,
                    item.TemplatePath,
                    item.ReportType,
                    item.ViewerType,
                    item.PaperWidth,
                    item.PaperHeight,
                    item.Orientation,
                    item.IsThermal,
                    item.IsLandscape,
                    item.DisplayOrder,
                    item.Icon,
                    item.CanView,
                    item.CanPrint,
                    item.ExportPdf,
                    item.ExportExcel,
                    TemplateExists = ReportLayoutAssetHelper.Exists(_environment, _configuration, item.TemplatePath)
                })
                .OrderBy(item => item.CategoryName)
                .ThenBy(item => item.DisplayOrder)
                .ThenBy(item => item.DisplayName)
                .Cast<object>()
                .ToList();
        }

        public IEnumerable<object> GetReportPermissions(int orgId, int roleId)
        {
            var result = (from report in _uow.GenericRepository<ReportMaster>().Table()
                          join category in _uow.GenericRepository<ReportCategory>().Table()
                              on report.CategoryId equals category.Id
                          join permission in _uow.GenericRepository<ReportPermission>().Table()
                                  .Where(x => x.OrgId == orgId && x.RoleId == roleId)
                              on report.Id equals permission.ReportId into permissionGroup
                          from permission in permissionGroup.DefaultIfEmpty()
                          where report.IsDeleted == false
                                && report.IsActive == true
                                && category.IsDeleted == false
                                && category.IsActive == true
                          orderby category.Name, report.DisplayOrder, report.DisplayName
                          select new
                          {
                              report.Id,
                              report.CategoryId,
                              CategoryName = category.Name,
                              report.ReportCode,
                              report.ReportName,
                              report.DisplayName,
                              CanView = permission != null && permission.CanView,
                              CanPrint = permission != null && permission.CanPrint,
                              ExportPdf = permission != null && permission.ExportPdf,
                              ExportExcel = permission != null && permission.ExportExcel
                          }).ToList<object>();

            return result;
        }

        public string SaveReportPermission(List<ReportPermission> reportPermissions)
        {
            if (reportPermissions == null || reportPermissions.Count == 0)
            {
                return string.Empty;
            }

            foreach (var permission in reportPermissions)
            {
                var existingPermission = _uow.GenericRepository<ReportPermission>()
                    .Table()
                    .FirstOrDefault(x => x.OrgId == permission.OrgId
                                      && x.RoleId == permission.RoleId
                                      && x.ReportId == permission.ReportId);

                if (existingPermission != null)
                {
                    existingPermission.CanView = permission.CanView;
                    existingPermission.CanPrint = permission.CanPrint;
                    existingPermission.ExportPdf = permission.ExportPdf;
                    existingPermission.ExportExcel = permission.ExportExcel;

                    _uow.GenericRepository<ReportPermission>().Update(existingPermission);
                }
                else
                {
                    var entity = new ReportPermission
                    {
                        OrgId = permission.OrgId,
                        RoleId = permission.RoleId,
                        ReportId = permission.ReportId,
                        CanView = permission.CanView,
                        CanPrint = permission.CanPrint,
                        ExportPdf = permission.ExportPdf,
                        ExportExcel = permission.ExportExcel
                    };

                    _uow.GenericRepository<ReportPermission>().Insert(entity);
                }
            }

            _uow.Save();

            return "Success";
        }

        public object? GetReportDefinition(int orgId, int roleId, int reportId)
        {
            var report = (from reportMaster in _uow.GenericRepository<ReportMaster>().Table()
                          join category in _uow.GenericRepository<ReportCategory>().Table()
                              on reportMaster.CategoryId equals category.Id
                          join permission in _uow.GenericRepository<ReportPermission>().Table()
                              on reportMaster.Id equals permission.ReportId
                          where reportMaster.Id == reportId
                                && reportMaster.OrgId == orgId
                                && category.OrgId == orgId
                                && permission.OrgId == orgId
                                && permission.RoleId == roleId
                                && permission.CanView == true
                                && reportMaster.IsDeleted == false
                                && reportMaster.IsActive == true
                                && category.IsDeleted == false
                                && category.IsActive == true
                          select new
                          {
                              reportMaster.Id,
                              reportMaster.OrgId,
                              reportMaster.CategoryId,
                              CategoryName = category.Name,
                              reportMaster.ReportCode,
                              reportMaster.ReportName,
                              reportMaster.DisplayName,
                              reportMaster.Description,
                              reportMaster.StoredProcedure,
                              reportMaster.TemplateName,
                              reportMaster.TemplatePath,
                              reportMaster.ReportType,
                              reportMaster.ViewerType,
                              reportMaster.PaperWidth,
                              reportMaster.PaperHeight,
                              reportMaster.Orientation,
                              reportMaster.IsThermal,
                              reportMaster.IsLandscape,
                              reportMaster.Icon,
                              permission.CanView,
                              permission.CanPrint,
                              permission.ExportPdf,
                              permission.ExportExcel
                          }).FirstOrDefault();

            if (report == null)
            {
                return null;
            }

            var filters = _uow.GenericRepository<ReportFilter>().Table()
                .Where(filter => filter.ReportId == reportId && filter.IsActive == true)
                .OrderBy(filter => filter.DisplayOrder)
                .Select(filter => new
                {
                    filter.Id,
                    filter.ReportId,
                    filter.FieldName,
                    filter.DisplayName,
                    filter.ControlType,
                    filter.DataType,
                    filter.IsRequired,
                    filter.IsActive,
                    filter.IsShow,
                    filter.DisplayOrder
                })
                .ToList();

            return new
            {
                report.Id,
                report.OrgId,
                report.CategoryId,
                report.CategoryName,
                report.ReportCode,
                report.ReportName,
                report.DisplayName,
                report.Description,
                report.StoredProcedure,
                report.TemplateName,
                report.TemplatePath,
                report.ReportType,
                report.ViewerType,
                report.PaperWidth,
                report.PaperHeight,
                report.Orientation,
                report.IsThermal,
                report.IsLandscape,
                report.Icon,
                report.CanView,
                report.CanPrint,
                report.ExportPdf,
                report.ExportExcel,
                TemplateExists = ReportLayoutAssetHelper.Exists(_environment, _configuration, report.TemplatePath),
                TemplateKind = ReportLayoutAssetHelper.ResolveTemplateKind(_environment, _configuration, report.TemplatePath),
                LayoutAsset = ReportLayoutAssetHelper.LoadJson(_environment, _configuration, report.TemplatePath),
                HtmlTemplate = ReportLayoutAssetHelper.LoadHtml(_environment, _configuration, report.TemplatePath),
                Filters = filters
            };
        }
    }
}
