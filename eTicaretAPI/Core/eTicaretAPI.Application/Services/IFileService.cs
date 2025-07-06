using Microsoft.AspNetCore.Http;

namespace eTicaretAPI.Application.Services;

public interface IFileService
{ 
    Task UploadAsync(string path, IFormFileCollection files);
}