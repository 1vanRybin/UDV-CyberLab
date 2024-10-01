﻿using Domain.Entities;
using ExampleCore.Helpers;
using IdentityServerApi.Controllers.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

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
    public async Task<ActionResult<User>> GetUser()
    {
        var userId = UserHelper.GetUserId(HttpContext.Request);
        var result = await _userService.GetUserInfoAsync(userId);
        if (result.user is null)
        {
            return NotFound("User Not Found");
        }
        
        return Ok(new UserInfoResponse
        {
            UserName = result.user.UserName,
            Email = result.user.Email,
            Role = result.role
        });
    }
    
    [HttpGet("users/{pageId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<User>>> GetUsers([FromRoute] int pageId)
    {
        return await _userService.GetUsersAsync(pageId);
    }
}