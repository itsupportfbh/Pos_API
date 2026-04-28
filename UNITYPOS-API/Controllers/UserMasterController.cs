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
    public class UserMasterController : ControllerBase
    {

        private readonly IUserMasterService _userMasterService;
        public UserMasterController(IUserMasterService userMasterService)
        {
            _userMasterService = userMasterService;
        }

        [HttpPost]
        public string Create(CreateUserMaster userMaster)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_userMasterService.Create(userMaster));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(CreateUserMaster userMaster)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_userMasterService.Update(userMaster));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetAllUsers(int OrgId)
        {
            string result = null;

            result= JsonConvert.SerializeObject(_userMasterService.GetAllUsers(OrgId));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetById(int Id)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_userMasterService.GetById(Id));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_userMasterService.Delete(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_userMasterService.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }

    }
}
