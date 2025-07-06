using eTicaretAPI.Application.Repositories;
using eTicaretAPI.Application.RequestParameter;
using eTicaretAPI.Application.Services;
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
        private readonly IFileService _fileService;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;

        public ProductsController(IProductReadRepositories productReadRepositories, 
                                  IProductWriteRepositories productWriteRepositories, 
                                  IWebHostEnvironment webHostEnvironment, 
                                  IFileService fileService, 
                                  IFileReadRepository fileReadRepository, 
                                  IFileWriteRepository fileWriteRepository, 
                                  IProductImageFileReadRepository productImageFileReadRepository, 
                                  IProductImageFileWriteRepository productImageFileWriteRepository, 
                                  IInvoiceFileReadRepository invoiceFileReadRepository, 
                                  IInvoiceFileWriteRepository invoiceFileWriteRepository)
        {
            _productReadRepositories = productReadRepositories;
            _productWriteRepositories = productWriteRepositories;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
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
            var datas = await _fileService.UploadAsync("resource/product-images", Request.Form.Files);
            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile() {
                FileName = d.fileName,
                FilePath = d.path,    
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();
            return Ok(); // Return 200 OK after uploading files
        }
    }
}
