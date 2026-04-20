using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;

namespace UNITYPOS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class MenuController : ControllerBase
    {

        private IMenuService _menuService;
        private readonly IUnitOfWork _uow;
        public MenuController(IMenuService menuService, IUnitOfWork uow)
        {
            _uow = uow;
            _menuService =menuService;
        }

        [HttpGet]
        public string GetAllMenuAndSubMenu()
        {
            string result = null;

            try
            {
                var data = _menuService.GetAllMenuAndSubMenu();
                result = JsonConvert.SerializeObject(data);
                return Common.Utility.GetResult(result);
            }
            catch (Exception ex)
            {
                result = JsonConvert.SerializeObject(new
                {
                    Status = false,
                    Message = "An error occurred while fetching menu and submenu details.",
                    Error = ex.Message
                });

                return Common.Utility.GetResult(result);
            }
        }
    }




}

