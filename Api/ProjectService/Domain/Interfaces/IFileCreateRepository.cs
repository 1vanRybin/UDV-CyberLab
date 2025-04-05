﻿using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces;

public interface IFileManager
{
    Task<string> CreateAsync(IFormFile? file, string projectDirectory, string fileName);
}