using AutoMapper;
using CRM.Data.Common.Exceptions;
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
        IFormFile documentation,
        Guid ownerId)
    {
        var card = _mapper.Map<ProjectCard>(cardDto);
        card.Id = Guid.NewGuid();

        var projectDirectory = $"uploads/{card.Id}";

        card.LogoPath = await _fileManager.CreateAsync(logo, projectDirectory, $"logo_{card.Name}");
        card.PhotoPath = await _fileManager.CreateAsync(photo, projectDirectory, $"photo_{card.Name}");
        card.DocumentationPath =
            await _fileManager.CreateAsync(documentation, projectDirectory, $"documentation_{card.Name}");
        card.OwnerId = ownerId;

        var cardId = await _projectRepository.CreateAsync(card.Id, card);

        return cardId;
    }

    public async Task<ProjectPageDto> GetByIdAsync(Guid id)
    {
        var card = await _projectRepository.GetByIdAsync<ProjectCard>(id);
        if (card == null)
        {
            throw new NotFoundException($"Project with id {id} didn't find.");
        }

        card.ViewsCount++;

        await _projectRepository.UpdateAsync(card);

        return _mapper.Map<ProjectPageDto>(card);
    }

    public async Task<ProjectCardFilesResponse> GetProjectFilesAsync(Guid projectId)
    {
        var card = await _projectRepository.GetByIdAsync<ProjectCard>(projectId);

        if (card == null)
            throw new FileNotFoundException($"Project with id {projectId} didn't find.");

        var response = new ProjectCardFilesResponse();

        if (!string.IsNullOrEmpty(card.LogoPath))
        {
            var (logoData, logoMime) = await _fileManager.GetFileWithMimeTypeAsync(card.LogoPath);
            response.Logo = logoData;
            response.LogoMimeType = logoMime;
        }

        if (!string.IsNullOrEmpty(card.PhotoPath))
        {
            var (photoData, photoMime) = await _fileManager.GetFileWithMimeTypeAsync(card.PhotoPath);
            response.Photo = photoData;
            response.PhotoMimeType = photoMime;
        }

        if (!string.IsNullOrEmpty(card.DocumentationPath))
        {
            var (docData, docMime) = await _fileManager.GetFileWithMimeTypeAsync(card.DocumentationPath);
            response.Documentation = docData;
            response.DocumentationMimeType = docMime;
        }

        return response;
    }

    public async Task<(byte[] Data, string MimeType)> GetProjectFileAsync(string path)
    {
        return await _fileManager.GetFileWithMimeTypeAsync(path);
    }

    public async Task<Guid> UpdateAsync(ProjectCardUpdateDto updateDto)
    {
        var existingCard = await _projectRepository.GetByIdAsync<ProjectCard>(updateDto.Id)
                           ?? throw new NotFoundException($"Project with id {updateDto.Id} not found.");

        _mapper.Map(updateDto, existingCard);

        await _projectRepository.UpdateAsync(existingCard);
        return existingCard.Id;
    }


    public async Task<ShortCardDto[]> GetFilteredProjectsAsync(ProjectFilterDto filter)
    {
        var projects = await _projectRepository.GetFilteredProjectsAsync(filter);

        var shortCards = _mapper.Map<ShortCardDto[]>(projects);

        return shortCards;
    }

    /// <summary>
    /// Увеличить счетчик посещений лендинга проекта
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    public async Task IncrementLandingVisitsCountAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync<ProjectCard>(projectId);
        if (project == null)
        {
            throw new NotFoundException($"Project with id {projectId} didn't find.");
        }

        project.LandingVisitsCount++;
        await _projectRepository.UpdateAsync(project);
    }
}