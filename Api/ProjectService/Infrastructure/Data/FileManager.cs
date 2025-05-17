using Amazon.S3.Model;
using Amazon.S3;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class FileManager : IFileManager
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public FileManager(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["YandexStorage:BucketName"];
    }

    public async Task<string> CreateAsync(IFormFile? file, string directoryPath, string fileName)
    {
        if (file == null || file.Length == 0)
            return string.Empty;

        var normalizedDirectory = directoryPath.Replace('\\', '/');
        var extension = Path.GetExtension(file.FileName);
        var objectKey = $"{normalizedDirectory}/{fileName}{extension}".TrimStart('/');

        using (var stream = file.OpenReadStream())
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            await _s3Client.PutObjectAsync(putRequest);
        }

        return objectKey;
    }

    public async Task<(byte[] Data, string MimeType)> GetFileWithMimeTypeAsync(string objectKey)
    {
        if (string.IsNullOrEmpty(objectKey))
            return (null, null);

        try
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey
            };

            using var response = await _s3Client.GetObjectAsync(getRequest);
            using var memoryStream = new MemoryStream();

            await response.ResponseStream.CopyToAsync(memoryStream);

            return (memoryStream.ToArray(), response.Headers.ContentType);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return (null, null);
        }
    }

    public async Task DeleteAsync(string objectKey)
    {
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
        }
        catch (AmazonS3Exception ex)
        {
            //_logger.LogError($"Error deleting file {objectKey}: {ex.Message}");
        }
    }
}