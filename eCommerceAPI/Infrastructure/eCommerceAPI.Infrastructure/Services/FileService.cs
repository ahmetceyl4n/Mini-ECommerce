using eCommerceAPI.Application.Services;
using eCommerceAPI.Infrastructure.StaticServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace eCommerceAPI.Infrastructure.Services;

public class FileService : IFileService
{
    readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
            
            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true; // Return true if the file copy was successful
        }
        catch (Exception ex)
        {
            // Log the exception (ex) if necessary
            throw ex; // Rethrow the exception or handle it as needed
        }
    }

    private async Task<string> FileRenameAsync(string path, string fileName, bool isFirst = true, int index = 0)
    {
        string fileExtension = Path.GetExtension(fileName);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string newFileName;

        if (isFirst)
        {
            fileNameWithoutExtension = NameOperation.CharacterRegulatory(fileNameWithoutExtension);
            newFileName = $"{fileNameWithoutExtension}{fileExtension}";
        }
        else
        {
            newFileName = $"{fileNameWithoutExtension}({index}){fileExtension}";
        }

        if (File.Exists(Path.Combine(path, newFileName)))
        {
            return await FileRenameAsync(path, fileName, false, index + 1); 
        }

        return newFileName;
    }


    public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
    {
        string uploadPath = Path.Combine(_environment.WebRootPath, path);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        List<(string fileName, string path)> datas = new();
        List<bool> results = new();

        foreach (IFormFile file in files)
        {
           
            string fileNewName = await FileRenameAsync(uploadPath, file.FileName);
            datas.Add((fileNewName, $"{path}\\{fileNewName}"));
            await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);           
        }

        if (results.TrueForAll(r => r))
        {
            return datas; // Return the list of file names and paths if all copies were successful
        }

        return null;
    }
}