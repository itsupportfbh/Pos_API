using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _uow;

        public AuthController(ITokenService tokenService, IUnitOfWork uow)
        {
            _tokenService = tokenService;
            _uow = uow;
        }

        [AllowAnonymous]
        [HttpPost]
       
        public async Task<string> Login([FromBody] LoginRequest request)
        {
            string result = null;

            try
            {
                if (request == null)
                    return ToResult(new { Status = false, Message = "Request body is required." });

                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                    return ToResult(new { Status = false, Message = "Email and Password are required." });

                var email = request.Email.Trim();

                var user = await _uow.GenericRepository<UserMaster>()
                                     .Table()
                                     .FirstOrDefaultAsync(x => x.Email == email && x.IsActive);

                if (user == null || user.Password != request.Password)
                    return ToResult(new { Status = false, Message = "Invalid email or password." });

                // 🔐 Generate Token
                var tokenResult = _tokenService.GenerateToken(user);

                // 👤 Prepare Current User Details (same like GetCurrentUser)
                var currentUser = new
                {
                    UserId = user.Id,
                    OrgId=user.OrgId,
                    Name = user.Name,
                    Email = user.Email,
                    IsAdmin = user.IsAdmin ,
                    EmpCode = user.EmpCode

                };

                // 🎯 Final Response (Token + User Details)
                return ToResult(new
                {
                   
                        Token = tokenResult.Token,
                        Expiration = tokenResult.Expiration,
                        User = currentUser
                    
                });
            }
            catch (Exception ex)
            {
                return ToResult(new
                {
                    Status = false,
                    Message = "An error occurred while logging in.",
                    Error = ex.Message
                });
            }
        }

        private string ToResult(object data)
        {
            string result = JsonConvert.SerializeObject(data);
            return Common.Utility.GetResult(result);
        }
    }
}