using Domain.DTO;
using Microsoft.AspNetCore.Http;

namespace Service.Interfaces;

public interface IProjectService
{
    Task<Guid> CreateAsync(ProjectCardDTO card,
        IFormFile logo,
        IFormFile? photo,
        IFormFile documentation);
}