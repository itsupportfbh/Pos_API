using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private IReservation _Resservice;
        private readonly IUnitOfWork _uow;
        public ReservationController(IReservation ResService, IUnitOfWork uow)
        {
            _uow = uow;
            _Resservice = ResService;
        }

        [HttpGet]
        public string GetAllReservation(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Resservice.GetAllReservation(orgid));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetReservationbyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Resservice.GetReservationbyId(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public string Create(Reservations reservations)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Resservice.Create(reservations));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string Update(Reservations reservations)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Resservice.Update(reservations));
            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_Resservice.DeleteById(id));
            return Common.Utility.GetResult(result);
        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_Resservice.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
