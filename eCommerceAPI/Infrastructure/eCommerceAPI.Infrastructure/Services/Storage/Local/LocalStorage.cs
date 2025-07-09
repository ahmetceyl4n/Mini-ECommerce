using eCommerceAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        readonly IWebHostEnvironment _environment;

        public LocalStorage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task DeleteAsync(string path, string fileName)
        
            =>  File.Delete($"{path}\\{fileName}"); // File.Delete metodu, belirtilen dosyayı silmek için kullanılır. Eğer dosya mevcut değilse, bir hata fırlatır.


        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);  //DirectoryInfo özelliği, belirtilen dizindeki dosyaları ve alt dizinleri yönetmek için kullanılır.
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
        
            => File.Exists($"{path}\\{fileName}"); // File.Exists metodu, belirtilen dosyanın var olup olmadığını kontrol eder. Eğer dosya mevcutsa true, aksi halde false döner.

        async Task<bool> CopyFileAsync(string path, IFormFile file)
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

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_environment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string fileName, string path)> datas = new();
            

            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(path, file.Name, HasFile); // dosya ismini yeniden adlandırma işlemi, eğer aynı isimde dosya varsa, isme (1), (2) gibi ekler

                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{path}\\{fileNewName}"));               
            }

            

            return datas;
        }
    }
}
