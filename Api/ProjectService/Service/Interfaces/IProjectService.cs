using Domain.DTO;
using Microsoft.AspNetCore.Http;

namespace Service.Interfaces;

public interface IProjectService
{
    Task<Guid> CreateAsync(ProjectCardDTO card,
        IFormFile logo,
        IFormFile? photo,
        IFormFile documentation,
        Guid ownerId);

    Task<ProjectPageDto> GetByIdAsync(Guid id);
    Task<ProjectCardFilesResponse> GetProjectFilesAsync(Guid projectId);
    Task<(byte[] Data, string MimeType)> GetProjectFileAsync(string path);
    Task<ShortCardDto[]> GetFilteredProjectsAsync(ProjectFilterDto filter);
    Task<Guid> UpdateAsync(ProjectCardUpdateDto updateDto);
    Task<ShortCardDto[]> GetUserProjects(Guid userId);
    Task<bool> DeleteProjectCardAsync(Guid cardId);
}