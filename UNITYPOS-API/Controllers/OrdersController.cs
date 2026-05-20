using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class OrdersController : ControllerBase
    {

        private IOrderService _orderService;
        private readonly IUnitOfWork _uow;
        public OrdersController(IOrderService orderService, IUnitOfWork uow)
        {
            _uow = uow;
            _orderService = orderService;
        }




        [HttpPost]
        public string Create(Orders orders)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderService.Create(orders));

            return Common.Utility.GetResult(result);
        }


        [HttpPut]
        public string Update(Orders orders)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderService.Update(orders));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetAll(long orgid, long branchId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_orderService.GetAllOrderDetails(orgid, branchId));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderService.GetById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderService.Delete(id));
            return Common.Utility.GetResult(result);

        }


        //[HttpGet]
        //public string GetAllHoldorderDetails(long orderId)
        //{
        //    string result = null;
        //    result = JsonConvert.SerializeObject(_orderService.GetAllHoldorderDetails(orderId));
        //    return Common.Utility.GetResult(result);
        //}
    }
}
