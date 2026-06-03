using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MoveTablesController : ControllerBase
    {
        private IMoveTable _Moveservice;
        private readonly IUnitOfWork _uow;
        public MoveTablesController(IMoveTable Moveservice, IUnitOfWork uow)
        {
            _uow = uow;
            _Moveservice = Moveservice;
        }

        [HttpGet]
        public string GetAllMoveTables(int orgid)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Moveservice.GetAllMoveTable(orgid));
            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetMoveTablebyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Moveservice.GetMoveTablebyId(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public string Create(MoveTables movetable)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Moveservice.Create(movetable));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(MoveTables movetable)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Moveservice.Update(movetable));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Moveservice.DeleteById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Moveservice.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
