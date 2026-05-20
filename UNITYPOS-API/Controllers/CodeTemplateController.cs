using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CodeTemplateController : ControllerBase
    {
        private readonly ICodeTemplateService _codeTemplateService;

        public CodeTemplateController(ICodeTemplateService codeTemplateService)
        {
            _codeTemplateService = codeTemplateService;
        }

        [HttpPost]
        public string CreateCodetemplate(List<CodeTemplate> codeTemplates)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_codeTemplateService.CreateCodetemplate(codeTemplates));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetAllCodeTemplate(int OrgId, int BranchId, bool IsMaster)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_codeTemplateService.GetAllCodeTemplate(OrgId, BranchId, IsMaster));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetLatestCode(int EntityNo, int OrgId, int BranchId)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_codeTemplateService.GetLatestCode(EntityNo, OrgId, BranchId));

            return Common.Utility.GetResult(result);
        }
    }
}
