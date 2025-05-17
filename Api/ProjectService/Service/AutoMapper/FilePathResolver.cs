using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Service.AutoMapper
{
    public class FilePathResolver : IValueResolver<ProjectCardUpdateDto, ProjectCard, string>
    {
        private readonly IFileManager _fileManager;
        private readonly Func<ProjectCardUpdateDto, IFormFile?> _fileSelector;
        private readonly Func<ProjectCard, string?> _currentPathSelector;
        private readonly Func<ProjectCard, string> _fileNameGenerator;

        public FilePathResolver(
            IFileManager fileManager,
            Func<ProjectCardUpdateDto, IFormFile?> fileSelector,
            Func<ProjectCard, string?> currentPathSelector,
            Func<ProjectCard, string> fileNameGenerator)
        {
            _fileManager = fileManager;
            _fileSelector = fileSelector;
            _currentPathSelector = currentPathSelector;
            _fileNameGenerator = fileNameGenerator;
        }

        public string Resolve(ProjectCardUpdateDto source, ProjectCard destination, string destMember, ResolutionContext context)
        {
            var newFile = _fileSelector(source);
            if (newFile == null || newFile.Length == 0)
                return _currentPathSelector(destination) ?? string.Empty;

            var currentPath = _currentPathSelector(destination);
            if (!string.IsNullOrEmpty(currentPath))
            {
                try
                {
                    _fileManager.DeleteAsync(currentPath).Wait();
                }
                catch
                {
                    //_logger.LogError($"Error resolving file {currentPath}: {ex.Message}");
                }
            }

            var projectDirectory = $"uploads/{destination.Id}";
            return _fileManager.CreateAsync(
                newFile,
                projectDirectory,
                _fileNameGenerator(destination)).Result;
        }
    }
}
