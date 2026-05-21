using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrderHoldController : ControllerBase
    {


        private IOrderHoldService _orderHoldService;
        private readonly IUnitOfWork _uow;
        public OrderHoldController(IOrderHoldService orderHoldService, IUnitOfWork uow)
        {
            _uow = uow;
            _orderHoldService = orderHoldService;
        }

        
        [HttpPost]
        public async Task<string> Create( OrdersHold ordershold)
        {
            var serviceResult = await _orderHoldService.Create(ordershold);

            return Common.Utility.GetResult(serviceResult);
        }
        [HttpPut]
        public async Task<string> Update( OrdersHold ordershold)
        {
            var result = await _orderHoldService.Update(ordershold);

            return Common.Utility.GetResult(JsonConvert.SerializeObject(result));
        }


        

        [HttpGet]
        public string GetAll(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_orderHoldService.GetAll(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderHoldService.GetById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderHoldService.Delete(id));
            return Common.Utility.GetResult(result);

        }


        [HttpGet]
        public string GetAllHoldorderDetails(long orderId)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_orderHoldService.GetAllHoldorderDetails(orderId));
            return Common.Utility.GetResult(result);
        }
    }
}
