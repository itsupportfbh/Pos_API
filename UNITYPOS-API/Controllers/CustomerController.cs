using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
     [Authorize]
    public class CustomerController : ControllerBase
    {

        private ICustomerService _customerservice;
        private readonly IUnitOfWork _uow;
        public CustomerController(ICustomerService customerService, IUnitOfWork uow)
        {
            _uow = uow;
            _customerservice = customerService;
        }


        [HttpGet]
        public string GetAllCustomer(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_customerservice.GetAllCustomer(orgid));

            return Common.Utility.GetResult(result);

        }

        [HttpGet]
        public string GetCustomerbyId(int id)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_customerservice.GetCustomerbyId(id));

            return Common.Utility.GetResult(result);

        }

        [HttpPost]
        public string Create(CustomerMaster customer)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_customerservice.Create(customer));

            return Common.Utility.GetResult(result);
        }


        [HttpPut]

        public string Update(CustomerMaster customer)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_customerservice.Update(customer));

            return Common.Utility.GetResult(result);
        }
        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_customerservice.DeleteById(id));

            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string ActiveInActive(int id, bool isActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_customerservice.ActiveInActive(id,isActive));

            return Common.Utility.GetResult(result);
        }
    }
}
