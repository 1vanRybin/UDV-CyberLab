using Domain.Entities;
using IdentityServerApi.Controllers.User.Request;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<int>> Register([FromBody] UserRegisterRequest request)
        {
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var registerResult = await _userService.RegisterUserAsync(new User()
            {
                UserName = request.FirstName,
                Email = request.Email
            }, request.Password);

            var user = new User()
            {
                Email = request.Email,
                Password = passwordHash,
                Role = request.Role,
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                IsApproved = true
            };
            try
            {
                await _userService.RegisterUserAsync(user, passwordHash);
            }
            catch (Exception e)
            {
                return Conflict(new { message = $"An existing user with email '{request.Email}' was already found." });
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var user = await _userService.GetAsync(request.Email);

            if (user == null)
            {
                return StatusCode(400, "User doesnt found");
            };

            if (user.IsApproved == false)
            {
                return StatusCode(403);
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            return Ok(new
            {
                token,
                user,
            });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("Id", user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
