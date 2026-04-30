using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CounterController : ControllerBase
    {
        private readonly ICounterService _counterService;
        public CounterController(ICounterService counterService)
        {
            _counterService = counterService;
        }



        [HttpGet]
        public string GetAllCounter(int orgId, string branchId)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_counterService.GetAllCounter(orgId, branchId));
            return Common.Utility.GetResult(result);

        }

        [HttpGet]
        public string GetCounterbyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_counterService.GetCounterbyId(id));
            return Common.Utility.GetResult(result);

        }
        [HttpPost]
        public string Create(Counter counter)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_counterService.Create(counter));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(Counter counter)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_counterService.Update(counter));
            return Common.Utility.GetResult(result);
        }
        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_counterService.DeleteById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_counterService.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
