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
    public class OrderHoldItemsController : ControllerBase
    {

        private IOrderHoldItemsService _orderHoldItemService;
        private readonly IUnitOfWork _uow;
        public OrderHoldItemsController(IOrderHoldItemsService orderHoldItemService, IUnitOfWork uow)
        {
            _uow = uow;
            _orderHoldItemService = orderHoldItemService;
        }



        [HttpPost]
        public string Create(OrderHoldItems ordersholditems)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderHoldItemService.Create(ordersholditems));

            return Common.Utility.GetResult(result);
        }


        [HttpPut]
        public string Update(OrderHoldItems ordersholditems)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderHoldItemService.Update(ordersholditems));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetAll(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_orderHoldItemService.GetAll(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderHoldItemService.GetById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderHoldItemService.Delete(id));
            return Common.Utility.GetResult(result);

        }
    }
}
