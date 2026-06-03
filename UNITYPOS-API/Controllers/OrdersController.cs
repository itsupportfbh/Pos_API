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
  //  [Authorize]
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
        public async Task<string> Create(Orders order)
        {
            var serviceResult = await _orderService.Create(order);

            return Common.Utility.GetResult(serviceResult);
        }
        [HttpPut]
        public async Task<string> Update(Orders order)
        {
            var result = await _orderService.Update(order);

            return Common.Utility.GetResult(JsonConvert.SerializeObject(result));
        }

        [HttpPut]
        public string KitchenStatusChange(Orders order)
        {
            var result =  _orderService.StatusChange(order);

            return Common.Utility.GetResult(JsonConvert.SerializeObject(result));
        }

        [HttpPut]
        public string KitchenItemStatusChange(Orderitems orderitems)
        {
            var result = _orderService.KitchenItemStatusChange(orderitems);

            return Common.Utility.GetResult(JsonConvert.SerializeObject(result));
        }





        [HttpGet]
        public string GetAll(int orgid, int branchId)
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
        [HttpGet]
        public string GetAllTable(int orgid ,int branchid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_orderService.GetAllDiningTable(orgid,branchid));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetTopSixMenuAndComboMenu(int orgid, int branchid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_orderService.GetTopSixMenuAndComboMenu(orgid, branchid));

            return Common.Utility.GetResult(result);
        }





        [HttpGet]

        public async Task<string> GetAllMenuAndComboMenu(int orgid,int branchid, int? categoryId,int? subCategoryId, string? searchKey = "")
        {
            var data = await _orderService.GetAllMenuAndComboMenu(orgid, branchid, categoryId, subCategoryId,searchKey);

            var result = JsonConvert.SerializeObject(data);

            return Common.Utility.GetResult(result);
        }

    }
}
