using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;
using IdentityServerApi.Controllers.User.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthorizeController : ControllerBase
{
    private readonly IUserService _userService;
    public AuthorizeController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpPost("register")]
    public async Task<ActionResult<int>> Register([FromBody] UserRegisterRequest model)
    {
        var registerResult = await _userService.RegisterUserAsync(new User 
            {
                UserName = model.UserName,
                Email = model.Email
            },
            model.Password,
            model.Role);

        if (registerResult.Succeeded)
        {
            return StatusCode(201);
        }

        foreach (var error in registerResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginRequest model)
    {
        var token = await _userService.LoginUserAsync(new User
        {
            Email = model.Email
        }, model.Password);

        if (token is null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }
}

