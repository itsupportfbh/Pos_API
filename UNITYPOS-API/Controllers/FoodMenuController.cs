using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    //  [Authorize]
    public class FoodMenuController : ControllerBase
    {
        private IFoodmenu _Foodservice;
        private readonly IUnitOfWork _uow;
        public FoodMenuController(IFoodmenu FoodService, IUnitOfWork uow)
        {
            _uow = uow;
            _Foodservice = FoodService;
        }
        [HttpGet]
        public string GetAllMenu(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Foodservice.GetAllMenu(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetMenubyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Foodservice.GetMenubyId(id));
            return Common.Utility.GetResult(result);
        }
        [HttpPost]

        public string Create(FoodMenu foodmenu)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Foodservice.Create(foodmenu));
            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string Update(FoodMenu foodmenu)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Foodservice.Update(foodmenu));
            return Common.Utility.GetResult(result);
        }
        [HttpDelete]

        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Foodservice.DeleteById(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Foodservice.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
