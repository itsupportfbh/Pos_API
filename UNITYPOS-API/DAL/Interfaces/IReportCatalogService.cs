using UNITYPOS_API.Entities.Reports;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IReportCatalogService
    {
        public IEnumerable<object> GetCategories(int orgId, int roleId);
        public IEnumerable<object> GetReports(int orgId, int roleId, int? categoryId);
        public object? GetReportDefinition(int orgId, int roleId, int reportId);
        public IEnumerable<object> GetReportPermissions(int orgId, int roleId);
        public string SaveReportPermission(List<ReportPermission> reportPermissions);
    }
}
