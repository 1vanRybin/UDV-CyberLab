using Domain.Entities;
using ExampleCore.Helpers;
using IdentityServerApi.Controllers.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers;

[Route("/api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<ActionResult<UserInfoResponse>> GetUser()
    {
        var userId = UserHelper.GetUserId(HttpContext.Request);
        var result = await _userService.GetUserInfoAsync(userId);
        if (result is null)
        {
            return NotFound("User Not Found");
        }

        return Ok(new UserInfoResponse
        {
            UserId = userId,
            UserName = result.UserName,
            Email = result.Email,
            Role = result.Role
        });
    }

    [HttpGet("users")]
    [Authorize]
    public ActionResult<ICollection<UserInfoResponse>> GetUsers()
    {
        var users = _userService.GetUsersAsync();
        return users.Select(user => new UserInfoResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role
        }).ToList();
    }
}
