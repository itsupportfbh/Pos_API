using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserBranchMappingController : ControllerBase
    {

        private readonly IUserBranchMappingService _userBranchMappingService;
        public UserBranchMappingController(IUserBranchMappingService userBranchMappingService)
        {
            _userBranchMappingService = userBranchMappingService;
        }

        [HttpPost]
        public string Create(List<UserBranchMapping> userBranchMapping)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_userBranchMappingService.Create(userBranchMapping));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(List<UserBranchMapping> userBranchMapping)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_userBranchMappingService.Update(userBranchMapping));

            return Common.Utility.GetResult(result);
        }

    }
}
