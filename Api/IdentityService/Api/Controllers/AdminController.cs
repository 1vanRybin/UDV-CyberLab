using IdentityServerApi.Controllers.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<ICollection<UserInfoResponse>>> SearchUsers([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name parameter is required.");

        var users = await _adminService.SearchUsersByNameAsync(name);
        var response = users.Select(user => new UserInfoResponse
        {
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = user.Role
        }).ToList();

        return Ok(response);
    }

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _adminService.DeleteUserAsync(userId);

        return NoContent();
    }
}