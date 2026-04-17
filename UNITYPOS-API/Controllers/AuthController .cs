using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new
                    {
                        Status = false,
                        Message = "Request body is required."
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new
                    {
                        Status = false,
                        Message = "Email and Password are required."
                    });
                }

                var email = request.Email.Trim();

                var user = await _uow.GenericRepository<UserMaster>()
                                     .Table()
                                     .FirstOrDefaultAsync(x => x.Email == email && x.IsActive);

                if (user == null)
                {
                    return Unauthorized(new
                    {
                        Status = false,
                        Message = "Invalid email or password."
                    });
                }

                // Plain text password check for now
                // Later you can replace with hashed password verification
                if (user.Password != request.Password)
                {
                    return Unauthorized(new
                    {
                        Status = false,
                        Message = "Invalid email or password."
                    });
                }

                var tokenResult = _tokenService.GenerateToken(user);

                var response = new LoginResponse
                {
                    Id = user.Id,
                    Code = user.Code,
                    Name = user.Name,
                    Email = user.Email,
                    EmpCode = user.EmpCode,
                    IsAdmin = user.IsAdmin,
                    Token = tokenResult.Token,
                    Expiration = tokenResult.Expiration
                };

                return Ok(new
                {
                    Status = true,
                    Message = "Login successful.",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = false,
                    Message = "An error occurred while logging in.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var empCode = User.FindFirst("EmpCode")?.Value;

            return Ok(new
            {
                Status = true,
                Data = new
                {
                    UserId = userId,
                    Name = name,
                    Email = email,
                    Role = role,
                    EmpCode = empCode
                }
            });
        }
    }
}