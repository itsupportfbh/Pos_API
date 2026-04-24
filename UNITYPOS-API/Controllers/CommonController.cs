using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Entities;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CommonController : ControllerBase
    {

        private readonly ICommonService _commonService;
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet]
        public string GetCountry()
        {
            string result = null;

            result= JsonConvert.SerializeObject(_commonService.GetCountry());

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetStateByCountryId(int CountryId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_commonService.GetStateByCountryId(CountryId));
            return Common.Utility.GetResult(result);
        }


        [HttpGet]
        public string GetCityByStateId(int StateId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_commonService.GetCityByStateId(StateId));
            return Common.Utility.GetResult(result);
        }

    }
}
