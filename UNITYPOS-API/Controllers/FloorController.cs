using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FloorController : ControllerBase
    {

        private IFloorService _floorService;
        private readonly IUnitOfWork _uow;
        public FloorController(IFloorService floorService, IUnitOfWork uow)
        {
            _uow = uow;
            _floorService = floorService;
        }

        [HttpPost]
        public string Create(FloorMaster floor)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_floorService.Create(floor));

            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(FloorMaster floor)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_floorService.Update(floor));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetAll(int orgid,int branchid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_floorService.GetAllFloor(orgid,branchid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_floorService.GetById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_floorService.Delete(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_floorService.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }

    }
}
