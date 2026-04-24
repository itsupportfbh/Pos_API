using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class SubCategoryController : ControllerBase
    {
        private IFoodSubCategory _FoodSubCategory;
        private readonly IUnitOfWork _uow;
        public SubCategoryController(IFoodSubCategory FoodSubCategory, IUnitOfWork uow)
        {
            _uow = uow;
            _FoodSubCategory = FoodSubCategory;
        }
        [HttpGet]
        public string GetAllSubCategory(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_FoodSubCategory.GetAllSubCategory(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetSubCategorybyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_FoodSubCategory.GetSubCategorybyId(id));
            return Common.Utility.GetResult(result);
        }
        [HttpPost]

        public string Create(FoodMenuSubCategory submenu)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_FoodSubCategory.Create(submenu));
            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string Update(FoodMenuSubCategory submenu)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_FoodSubCategory.Update(submenu));
            return Common.Utility.GetResult(result);
        }
        [HttpDelete]

        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_FoodSubCategory.DeleteById(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_FoodSubCategory.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
