using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Service.Interfaces;

namespace Service.Services;

public class ProjectService(
    IFileManager _fileManager,
    IProjectRepository _projectRepository,
    IMapper _mapper)
    : IProjectService
{
    public async Task<Guid> CreateAsync(ProjectCardDTO cardDto,
        IFormFile logo,
        IFormFile? photo,
        IFormFile documentation)
    {
        var card = _mapper.Map<ProjectCard>(cardDto);

        card.Id = Guid.NewGuid();

        var projectDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "uploads",
            "projects",
            card.Id.ToString());

        card.LogoPath = await _fileManager.CreateAsync(logo, projectDirectory, $"logo_{card.Name}");
        card.PhotoPath = await _fileManager.CreateAsync(photo, projectDirectory, $"photo_{card.Name}");
        card.DocumentationPath =
            await _fileManager.CreateAsync(documentation, projectDirectory, $"documentation_{card.Name}");

        var cardId = await _projectRepository.CreateAsync(card.Id, card);

        return cardId;
    }
}