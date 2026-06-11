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
    public class BillingController : ControllerBase
    {

        private IBillingService _billingservice;
        private readonly IUnitOfWork _uow;
        public BillingController(IBillingService billingservice, IUnitOfWork uow)
        {
            _uow = uow;
            _billingservice = billingservice;
        }

        [HttpPost]
        public string Create(Billing billing)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_billingservice.Create(billing));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(Billing billing)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_billingservice.Update(billing));

            return Common.Utility.GetResult(result);
        }



    }
}
