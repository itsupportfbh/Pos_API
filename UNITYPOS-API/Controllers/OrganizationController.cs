using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {

        private readonly IOrganizationService _organizationservice;
        private readonly ICommonService _commonService;

        public OrganizationController(IOrganizationService organizationservice, ICommonService commonService)
        {
            _organizationservice = organizationservice;
            _commonService = commonService;
        }

        [HttpPost]
        public string Create(Organization organization)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_organizationservice.Create(organization));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(Organization organization)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_organizationservice.Update(organization));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetAllOrganization()
        {
            string result = null;

            result= JsonConvert.SerializeObject(_organizationservice.GetAllOrganization());

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetById(int Id)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_organizationservice.GetById(Id));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_organizationservice.Delete(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_organizationservice.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public async Task<string> CreateUpdateOrganizationConfig([FromForm] OrganizationConfig organizationconfig)
        {
            if (organizationconfig.ImageFile != null)
            {
                var uploadResult = await _commonService.FileUpload(organizationconfig.ImageFile, "Organization");
                organizationconfig.Image = uploadResult.FileName;
            }

            string result = null;
            result = JsonConvert.SerializeObject(_organizationservice.CreateUpdateOrganizationConfig(organizationconfig));

            return Common.Utility.GetResult(result);
        }


        [HttpGet]
        public string GetOrganizationConfigByOrgId(int OrgId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_organizationservice.GetOrganizationConfigByOrgId(OrgId));
            return Common.Utility.GetResult(result);
        }

    }
}
