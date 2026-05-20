using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _uow;
        private readonly IAuthService _AuthService;
        public AuthController(ITokenService tokenService, IUnitOfWork uow, IAuthService AuthService)
        {
            _tokenService = tokenService;
            _uow = uow;
            _AuthService = AuthService;
        }

        [HttpPost]
        public async Task<string> Login([FromBody] LoginRequest request)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_AuthService.Login(request.Email, request.Password));

            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public async Task<string> Loginwithpin([FromBody] LoginWithPin request)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_AuthService.Login(request.Pin));

            return Common.Utility.GetResult(result);
        }
    }
}