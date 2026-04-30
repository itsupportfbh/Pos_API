using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PrinterController : ControllerBase
    {

        private IPrinterService _printerService;
        private readonly IUnitOfWork _uow;
        public PrinterController(IPrinterService printerService, IUnitOfWork uow)
        {
            _uow = uow;
            _printerService = printerService;
        }

        [HttpGet]
        public string GetAll(int orgid, int branchid, int counterid, int terminalid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_printerService.GetAll(orgid,branchid,counterid,terminalid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetById(int Id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_printerService.GetById(Id));
            return Common.Utility.GetResult(result);
        }
        [HttpPost]
        public string Create(Printer printer)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_printerService.Create(printer));
            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string Update(Printer printer)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_printerService.Update(printer));
            return Common.Utility.GetResult(result);
        }
        [HttpDelete]
        public string Delete(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_printerService.DeleteById(id));
            return Common.Utility.GetResult(result);

        }


        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_printerService.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }

    }
}
