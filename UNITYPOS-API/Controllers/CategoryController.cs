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
    public class CategoryController : ControllerBase
    {
        private IFoodCategory _FoodCategory;
        private readonly IUnitOfWork _uow;
        public CategoryController(IFoodCategory FoodCategory, IUnitOfWork uow)
        {
            _uow = uow;
            _FoodCategory = FoodCategory;
        }
        [HttpGet]
        public string GetAllCategory(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_FoodCategory.GetAllCategory(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetCategorybyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_FoodCategory.GetCategorybyId(id));
            return Common.Utility.GetResult(result);
        }
        [HttpPost]

        public string Create(FoodMenuCategory foodmenu)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_FoodCategory.Create(foodmenu));
            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string Update(FoodMenuCategory foodmenu)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_FoodCategory.Update(foodmenu));
            return Common.Utility.GetResult(result);
        }
        [HttpDelete]

        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_FoodCategory.DeleteById(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_FoodCategory.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
