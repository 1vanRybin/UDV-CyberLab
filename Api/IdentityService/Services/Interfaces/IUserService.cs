﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityResult> RegisterUserAsync(User user, string Password);
        public Task<JwtSecurityToken> LoginUserAsync(User userLogin, string password);

        public Task<User> GetUserInfoAsync(Guid userId);

        public Task<Guid> CheckExistAsync(Guid id);

        public Task<User> UpdateAsync(User user);
    }
}
