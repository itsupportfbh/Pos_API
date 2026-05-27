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
    public class JoinTablesController : ControllerBase
    {
        private IJoinTables _Joinservice;
        private readonly IUnitOfWork _uow;
        public JoinTablesController(IJoinTables Joinservice, IUnitOfWork uow)
        {
            _uow = uow;
            _Joinservice = Joinservice;
        }

        [HttpGet]
        public string GetAllJoinTables(int orgid)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Joinservice.GetAllJoinTable(orgid));
            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetJoinTablebyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Joinservice.GetJoinTablebyId(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public string Create(JoinTables jointable)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Joinservice.Create(jointable));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(JoinTables jointable)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Joinservice.Update(jointable));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Joinservice.DeleteById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Joinservice.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
