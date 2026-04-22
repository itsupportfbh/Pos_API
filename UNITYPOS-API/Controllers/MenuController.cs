using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Data.ORM;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {

        private IMenuService _menuService;
        private readonly IUnitOfWork _uow;
        public MenuController(IMenuService menuService, IUnitOfWork uow)
        {
            _uow = uow;
            _menuService = menuService;
        }

        [HttpGet]
        public string GetAllMenuAndSubMenu()
        {
            string result = null;

            result = JsonConvert.SerializeObject(_menuService.GetAllMenuAndSubMenu());

            return Common.Utility.GetResult(result);
        }
    }




}

