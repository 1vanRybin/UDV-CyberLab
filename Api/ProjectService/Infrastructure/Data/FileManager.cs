using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Data;

public class FileManager : IFileManager
{
    public async Task<string> CreateAsync(IFormFile? file, string projectDirectory, string fileName)
    {
        if (file is null) return "";

        Directory.CreateDirectory(projectDirectory);
        var extention = Path.GetExtension(file.FileName);
        var filePath = Path.Combine(projectDirectory, $"{fileName}{extention}");

        await using var fileStream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(fileStream);

        return filePath;
    }

    public async Task<(byte[] Data, string MimeType)> GetFileWithMimeTypeAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return (null, null);

        var fileBytes = await File.ReadAllBytesAsync(filePath);
        var mimeType = GetMimeType(filePath);

        return (fileBytes, mimeType);
    }

    private string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }
}