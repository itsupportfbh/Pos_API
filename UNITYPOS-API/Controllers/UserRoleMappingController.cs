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
    public class UserRoleMappingController : ControllerBase
    {

        private readonly IUserRoleMappingService _userRoleMappingService;
        public UserRoleMappingController(IUserRoleMappingService userRoleMappingService)
        {
            _userRoleMappingService = userRoleMappingService;
        }

        [HttpPost]
        public string Create(List<UserRoleMapping> userRoleMapping)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_userRoleMappingService.Create(userRoleMapping));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(List<UserRoleMapping> userRoleMapping)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_userRoleMappingService.Update(userRoleMapping));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetByUserId(int UserId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_userRoleMappingService.GetByUserId(UserId));
            return Common.Utility.GetResult(result);
        }

    }
}
