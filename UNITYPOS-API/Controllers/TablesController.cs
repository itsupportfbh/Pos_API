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
    public class TablesController : ControllerBase
    {
        private IDiningTable _Tableservice;
        private readonly IUnitOfWork _uow;
        public TablesController(IDiningTable TableService, IUnitOfWork uow)
        {
            _uow = uow;
            _Tableservice = TableService;
        }
        [HttpGet]
        public string GetAllTable(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Tableservice.GetAllDiningTable(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetTablebyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Tableservice.GetDiningTablebyId(id));
            return Common.Utility.GetResult(result);
        }
        [HttpPost]
        public string Create(DiningTableMaster Table)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Tableservice.Create(Table));
            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string Update(DiningTableMaster Table)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Tableservice.Update(Table));
            return Common.Utility.GetResult(result);
        }
        [HttpDelete]

        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Tableservice.DeleteById(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Tableservice.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
