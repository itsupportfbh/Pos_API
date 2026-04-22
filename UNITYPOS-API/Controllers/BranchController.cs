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
    public class BranchController : ControllerBase
    {
        private IBranchService _branchservice;
        private readonly IUnitOfWork _uow;
        public BranchController(IBranchService branchService, IUnitOfWork uow)
        {
            _uow = uow;
            _branchservice=branchService;
        }
        [HttpGet]
        public string GetAllBranch(int orgid)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_branchservice.GetAllBranch(orgid));

            return Common.Utility.GetResult(result);
        }
        [HttpGet]
        public string GetBranchbyId(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_branchservice.GetBranchbyId(id));
            return Common.Utility.GetResult(result);
        }
        [HttpPost]

        public string Create(Branch branch)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_branchservice.Create(branch));
            return Common.Utility.GetResult(result);
        }
        [HttpPut]
        public string Update(Branch branch)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_branchservice.Update(branch));
            return Common.Utility.GetResult(result);
        }
        [HttpDelete]

        public string DeleteById(int id)
        {
            string result = null;
            result = JsonConvert.SerializeObject(_branchservice.DeleteById(id));
            return Common.Utility.GetResult(result);

        }

        [HttpPut]
        public string ActiveInActive(int Id, bool IsActive)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_branchservice.ActiveInActive(Id, IsActive));
            return Common.Utility.GetResult(result);
        }
    }
}
