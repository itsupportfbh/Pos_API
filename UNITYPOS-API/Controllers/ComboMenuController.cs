using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ComboMenuController : ControllerBase
    {
        private readonly IComboMenu _combomenu;
        private readonly IUnitOfWork _uow;
        public ComboMenuController(IComboMenu comboMenu,IUnitOfWork uow)
        {
            _combomenu=comboMenu;
            _uow=uow;
        }

        [HttpGet]
        public string GetAllComboMenu(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_combomenu.GetAllComboMenu(orgid));

            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public string Create(ComboMenu comboMenu)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_combomenu.Create(comboMenu));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(ComboMenu comboMenu)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_combomenu.Update(comboMenu));

            return Common.Utility.GetResult(result);
        }


        [HttpGet]
        public string GetComboMenubyId(int id)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_combomenu.GetComboMenubyId(id));

            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_combomenu.DeleteById(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_combomenu.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
