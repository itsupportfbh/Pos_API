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
    
    public class CounterController : ControllerBase
    {
        private readonly ICounterService _counterService;
        public CounterController(ICounterService counterService)
        {
            _counterService = counterService;
        }



        [HttpGet]
        public string GetAllCounter(int orgId, int branchId)
        {
            string result = null;
            result= JsonConvert.SerializeObject(_counterService.GetAllCounter(orgId,branchId));
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


    }
}
