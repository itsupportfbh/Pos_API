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

    public class EntityMasterController : ControllerBase
    {
        private readonly IEntityMasterService _entitymasterservice;

        public EntityMasterController(IEntityMasterService entitymasterService)
        {
            _entitymasterservice = entitymasterService;
        }

        [HttpGet]
        public string GetEntityMasterForRoleRights(int OrgId, int RoleId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_entitymasterservice.GetEntityMasterForRoleRights(OrgId, RoleId));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public string SaveRolePermission(List<RolePermission> rolePermissions)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_entitymasterservice.SaveRolePermission(rolePermissions));
            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetRoleRightsByRoleId(int OrgId, int RoleId, int EntityNo)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_entitymasterservice.GetRoleRightsByRoleId(OrgId, RoleId, EntityNo));
            return Common.Utility.GetResult(result);
        }
    }
}
