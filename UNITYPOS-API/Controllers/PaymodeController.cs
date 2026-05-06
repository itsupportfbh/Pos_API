using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]

    public class PaymodeController : ControllerBase
    {
        private IPaymodeService _paymodeService;
        private readonly IUnitOfWork _uow;
        public PaymodeController(IPaymodeService paymodeService, IUnitOfWork uow)
        {
            _uow = uow;
            _paymodeService = paymodeService;
        }


        [HttpPost]
        public string Create(PaymodeMaster paymode)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_paymodeService.Create(paymode));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(PaymodeMaster paymode)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_paymodeService.Update(paymode));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetAll(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_paymodeService.GetAll(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_paymodeService.GetById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_paymodeService.Delete(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_paymodeService.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }

    }
}
