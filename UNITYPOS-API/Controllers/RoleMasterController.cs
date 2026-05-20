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
    public class RoleMasterController : ControllerBase
    {
        private readonly IRoleMasterService _roleMasterService;

        public RoleMasterController(IRoleMasterService roleMasterService)
        {
            _roleMasterService = roleMasterService;
        }

        [HttpGet]
        public string GetAllRole(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_roleMasterService.GetAllRole(orgid));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetRoleById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_roleMasterService.GetRoleById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public string Create(RoleMaster roleMaster)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_roleMasterService.Create(roleMaster));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(RoleMaster roleMaster)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_roleMasterService.Update(roleMaster));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_roleMasterService.DeleteById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int id, bool isActive)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_roleMasterService.ActiveInActive(id, isActive));
            return Common.Utility.GetResult(result);
        }
    }
}
