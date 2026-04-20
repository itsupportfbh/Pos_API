using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UNITYPOS_API.DAL.Interfaces;

namespace UNITYPOS_API.Controllers.Master
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





    }
}
