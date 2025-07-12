using eCommerceAPI.Application.Abstractions.Storage;
using eCommerceAPI.Application.Features.Commands.CreateProduct;
using eCommerceAPI.Application.Features.Queries.GetAllProduct;
using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Application.RequestParameter;
using eCommerceAPI.Application.ViewModels.Products;
using eCommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace eCommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepositories _productReadRepositories;
        private readonly IProductWriteRepositories _productWriteRepositories;
        private readonly IWebHostEnvironment _webHostEnvironment;        
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ProductsController(IProductReadRepositories productReadRepositories,
                                  IProductWriteRepositories productWriteRepositories,
                                  IWebHostEnvironment webHostEnvironment,
                                  IFileReadRepository fileReadRepository,
                                  IFileWriteRepository fileWriteRepository,
                                  IProductImageFileReadRepository productImageFileReadRepository,
                                  IProductImageFileWriteRepository productImageFileWriteRepository,
                                  IInvoiceFileReadRepository invoiceFileReadRepository,
                                  IInvoiceFileWriteRepository invoiceFileWriteRepository,
                                  IStorageService storageService,
                                  IConfiguration configuration,
                                  IMediator mediator)
        {
            _productReadRepositories = productReadRepositories;
            _productWriteRepositories = productWriteRepositories;
            _webHostEnvironment = webHostEnvironment;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);// Return the response from the mediator
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(_productReadRepositories.GetByIdAsync(id, false)); // Get product by ID with no tracking
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            if (response == null)
            {
                return BadRequest("Product creation failed."); // Return 400 Bad Request if creation fails
            }

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
        public async Task<IActionResult> Upload(string id){
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", Request.Form.Files);
            Product product = await _productReadRepositories.GetByIdAsync(id);

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
            {
                FileName = r.fileName,
                FilePath = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product } // Ürün ile ilişkilendiriyoruz
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();
   
            return Ok(); // Return 200 OK after uploading files
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepositories.Table.Include(p => p.productImageFiles).FirstOrDefaultAsync(p => p.ID == Guid.Parse(id)); // Ürün ID'sine göre ürünü alıyoruz

            return Ok(product.productImageFiles.Select(p => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.FilePath}",
                p.FileName,
                p.ID
            }));
        }

        [HttpDelete("[action]/{productId}")]
        public async Task<IActionResult> DeleteProductImage(string productId, string imageId)
        {
            Product? product = await _productReadRepositories.Table.Include(p => p.productImageFiles).FirstOrDefaultAsync(p => p.ID == Guid.Parse(productId)); // Ürün ID'sine göre ürünü alıyoruz

            ProductImageFile productImageFile = product.productImageFiles.FirstOrDefault(p => p.ID == Guid.Parse(imageId));

            product.productImageFiles.Remove(productImageFile);

            await _productWriteRepositories.SaveAsync();

            return Ok();
        }
    }
}
