using eTicaretAPI.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace eTicaretAPI.Infrastructure.Services;

public class FileService : IFileService
{
    readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public Task UploadAsync(string path, IFormFileCollection files)
    {
        string uploadPath = Path.Combine(_environment.WebRootPath, path);
    }
}