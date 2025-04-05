using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Data;

public class FileManager : IFileManager
{
    public async Task<string> CreateAsync(IFormFile? file, string projectDirectory, string fileName)
    {
        if (file is null) return "";

        Directory.CreateDirectory(projectDirectory);

        var filePath = Path.Combine(projectDirectory, fileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(fileStream);

        return filePath;
    }
}