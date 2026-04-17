using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;

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

        public IEnumerable<Object> GetAllBranch(int orgid)
        {

        }



    }
}
