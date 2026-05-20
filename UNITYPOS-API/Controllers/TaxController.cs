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
    public class TaxController : ControllerBase
    {
        private ITax _Tax;
        private readonly IUnitOfWork _uow;
        public TaxController(ITax Tax, IUnitOfWork uow)
        {
            _uow = uow;
            _Tax = Tax;
        }
        [HttpGet]
        public string GetAllTax(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Tax.GetAllTax(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetTaxbyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Tax.GetTaxbyId(id));
            return Common.Utility.GetResult(result);
        }
        [HttpPost]

        public string Create(Tax tax)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Tax.Create(tax));
            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string Update(Tax tax)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Tax.Update(tax));
            return Common.Utility.GetResult(result);
        }
        [HttpDelete]

        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Tax.DeleteById(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Tax.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
