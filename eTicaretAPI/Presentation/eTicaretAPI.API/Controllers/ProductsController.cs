using eTicaretAPI.Application.Repositories;
using eTicaretAPI.Application.RequestParameter;
using eTicaretAPI.Application.ViewModels.Products;
using eTicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace eTicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepositories _productReadRepositories;
        private readonly IProductWriteRepositories _productWriteRepositories;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(IProductReadRepositories productReadRepositories, IProductWriteRepositories productWriteRepositories, IWebHostEnvironment webHostEnvironment)
        {
            _productReadRepositories = productReadRepositories;
            _productWriteRepositories = productWriteRepositories;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Pagination pagination)
        {
            var totalCount = _productReadRepositories.GetAll(false).Count();
            var products = _productReadRepositories.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.ID,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            });
            
            return Ok(new
            {
                totalCount,
                products
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(_productReadRepositories.GetByIdAsync(id, false)); // Get product by ID with no tracking
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            if (ModelState.IsValid)
            { // Check if the model state is valid
               
             }

            await _productWriteRepositories.AddAsync(new()
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price
            });     // Create a new product entity
            await _productWriteRepositories.SaveAsync();
            return (StatusCode((int)HttpStatusCode.Created));   // Created response
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepositories.GetByIdAsync(model.id);
            if (product == null)
            {
                return NotFound(); // Return 404 if product not found
            }
            product.Name = model.Name;
            product.Stock = model.Stock;
            product.Price = model.Price;
            await _productWriteRepositories.SaveAsync();
            return NoContent(); // Return 204 No Content on successful update
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _productWriteRepositories.RemoveAsync(id);
            if (!success)
                return NotFound(); // ID yoksa veya geçersizse 404

            await _productWriteRepositories.SaveAsync();
            return NoContent(); // 204
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(){
            //wwwroot/resource/product-images klasörüne resim yükleme işlemi yapılacak
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (IFormFile file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false)) 
                    {
                        await file.CopyToAsync(fileStream); // Resmi belirtilen yola kopyala
                        await fileStream.FlushAsync(); // Stream'i temizle
                    } 
                }
            }
            return Ok(new { message = "Resimler başarıyla yüklendi." }); // Başarılı yükleme mesajı
        }
    }
}
