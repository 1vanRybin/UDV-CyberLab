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


    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _adminService.DeleteUserAsync(userId);

        return NoContent();
    }
}