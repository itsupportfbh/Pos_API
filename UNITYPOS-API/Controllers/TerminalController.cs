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
    public class TerminalController : ControllerBase
    {

        private ITerminalService _terminalService;
        private readonly IUnitOfWork _uow;
        public TerminalController(ITerminalService terminalService, IUnitOfWork uow)
        {
            _uow = uow;
            _terminalService = terminalService;
        }

        [HttpGet]
        public string GetAllTerminal(int orgid, string branchid, string counterid)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_terminalService.GetAllTerminal(orgid,branchid,counterid));

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetTerminalbyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_terminalService.GetTerminalbyId(id));

            return Common.Utility.GetResult(result);
        }
        [HttpPost]
        public string Create(Terminal terminal)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_terminalService.Create(terminal));

            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string Update(Terminal terminal)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_terminalService.Update(terminal));

            return Common.Utility.GetResult(result);
        }

        [HttpDelete]

        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_terminalService.DeleteById(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_terminalService.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
