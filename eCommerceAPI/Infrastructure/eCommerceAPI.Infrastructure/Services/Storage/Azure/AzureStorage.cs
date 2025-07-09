using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using eCommerceAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobContainerClient;
       
        public AzureStorage(IConfiguration configuration)   
        {
            _blobServiceClient = new(configuration["Storage:Azure"]); // Azure Blob Storage connection string
        } 

        public async Task DeleteAsync(string ContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName); // ContainerName, Azure Blob Storage'de oluşturulacak konteyner ismi
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName); // fileName, silinecek dosyanın ismi
            await blobClient.DeleteIfExistsAsync(); // dosya var ise silme işlemi yapılır
        }

        public List<string> GetFiles(string ContainerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName); // ContainerName, Azure Blob Storage'de oluşturulacak konteyner ismi
            return _blobContainerClient.GetBlobs().Select(blobItem => blobItem.Name).ToList(); // konteyner içindeki dosyaların isimlerini liste olarak döndürür
        }

        public bool HasFile(string ContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName); // ContainerName, Azure Blob Storage'de oluşturulacak konteyner ismi
            return _blobContainerClient.GetBlobs().Any(blobItem => blobItem.Name == fileName); // konteyner içindeki dosyaların isimlerini kontrol eder, eğer dosya varsa true, yoksa false döner
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string ContainerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);    // ContainerName, Azure Blob Storage'de oluşturulacak konteyner ismi
            await _blobContainerClient.CreateIfNotExistsAsync();                                // konteyner var ise oluşturma işlemi yapılmaz, yoksa oluşturulur
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);    // konteynerin erişim politikasını BlobContainer olarak ayarlıyoruz, böylece konteyner içindeki dosyalar herkese açık hale gelir

            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(ContainerName, file.Name, HasFile); // dosya ismini yeniden adlandırma işlemi, eğer aynı isimde dosya varsa, isme (1), (2) gibi ekler

                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName); // file.Name dosya yüklenirken kullanılacak isimdir
                await blobClient.UploadAsync(file.OpenReadStream());  // dosya yükleme işlemi
                datas.Add((fileNewName, ContainerName));                  // data olarak dosya ismi ve konteyner ismini ekliyoruz
            }
            return datas;  
        }
    }
}
