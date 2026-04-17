using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Entities;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {

        private readonly IOrganizationservice _organizationservice;
        public OrganizationController(IOrganizationservice organizationservice)
        {
            _organizationservice = organizationservice;
        }

        [HttpPost]
        public string Create(OrganizationDTO organizationDTO)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_organizationservice.Create(organizationDTO));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(OrganizationDTO organizationDTO)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_organizationservice.Update(organizationDTO));

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
        public string DeleteId(int id)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_organizationservice.DeleteById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_organizationservice.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }

    }
}
