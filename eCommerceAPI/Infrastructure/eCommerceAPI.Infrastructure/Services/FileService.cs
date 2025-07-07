using eCommerceAPI.Infrastructure.StaticServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace eCommerceAPI.Infrastructure.Services;

public class FileService
{
    readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
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
}