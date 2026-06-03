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
    public class ReportRuntimeController : ControllerBase
    {
        private readonly IReportRuntimeService _reportRuntimeService;

        public ReportRuntimeController(IReportRuntimeService reportRuntimeService)
        {
            _reportRuntimeService = reportRuntimeService;
        }

        [HttpPost]
        public async Task<string> Preview([FromBody] ReportExecutionRequest request)
        {
            string result = JsonConvert.SerializeObject(await _reportRuntimeService.ExecuteAsync(request));
            return Common.Utility.GetResult(result);
        }
    }
}
