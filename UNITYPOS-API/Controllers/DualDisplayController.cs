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
    public class DualDisplayController : ControllerBase
    {
        private readonly IDualDisplayService _dualDisplayService;

        public DualDisplayController(IDualDisplayService dualDisplayService)
        {
            _dualDisplayService = dualDisplayService;
        }

        [HttpGet]
        public string GetAll(int orgId)
        {
            string result = JsonConvert.SerializeObject(_dualDisplayService.GetAll(orgId));
            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetActiveProfile(int orgId, int branchId, int counterId = 0)
        {
            string result = JsonConvert.SerializeObject(_dualDisplayService.GetActiveProfile(orgId, branchId, counterId));
            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetById(int id)
        {
            string result = JsonConvert.SerializeObject(_dualDisplayService.GetById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public string Create(DualDisplayProfile profile)
        {
            string result = JsonConvert.SerializeObject(_dualDisplayService.Create(profile));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(DualDisplayProfile profile)
        {
            string result = JsonConvert.SerializeObject(_dualDisplayService.Update(profile));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = JsonConvert.SerializeObject(_dualDisplayService.DeleteById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int id, bool isActive)
        {
            string result = JsonConvert.SerializeObject(_dualDisplayService.ActiveInActive(id, isActive));
            return Common.Utility.GetResult(result);
        }
    }
}
