using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Entities.Reports;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ReportCatalogController : ControllerBase
    {
        private readonly IReportCatalogService _reportCatalogService;

        public ReportCatalogController(IReportCatalogService reportCatalogService)
        {
            _reportCatalogService = reportCatalogService;
        }

        [HttpGet]
        public string GetCategories(int OrgId, int RoleId)
        {
            string result = JsonConvert.SerializeObject(_reportCatalogService.GetCategories(OrgId, RoleId));
            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetReports(int OrgId, int RoleId, int? CategoryId = null)
        {
            string result = JsonConvert.SerializeObject(_reportCatalogService.GetReports(OrgId, RoleId, CategoryId));
            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetReportPermissions(int OrgId, int RoleId)
        {
            string result = JsonConvert.SerializeObject(_reportCatalogService.GetReportPermissions(OrgId, RoleId));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public string SaveReportPermission(List<ReportPermission> reportPermissions)
        {
            string result = JsonConvert.SerializeObject(_reportCatalogService.SaveReportPermission(reportPermissions));
            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetReportDefinition(int OrgId, int RoleId, int ReportId)
        {
            string result = JsonConvert.SerializeObject(_reportCatalogService.GetReportDefinition(OrgId, RoleId, ReportId));
            return Common.Utility.GetResult(result);
        }
    }
}
