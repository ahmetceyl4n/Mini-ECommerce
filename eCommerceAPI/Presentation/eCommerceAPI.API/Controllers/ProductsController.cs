using eCommerceAPI.Application.Abstractions.Storage;
using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Application.RequestParameter;
using eCommerceAPI.Application.ViewModels.Products;
using eCommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using eCommerceAPI.Application.Features.Product.Commands.RemoveProduct;
using eCommerceAPI.Application.Features.Product.Commands.CreateProduct;
using eCommerceAPI.Application.Features.Product.Commands.UpdateProduct;
using eCommerceAPI.Application.Features.Product.Queries.GetByIdProduct;
using eCommerceAPI.Application.Features.Product.Queries.GetAllProduct;
using eCommerceAPI.Application.Features.ProductImageFile.Commands.UploadProductImage;
using eCommerceAPI.Application.Features.ProductImageFile.Commands.RemoveProductImage;
using eCommerceAPI.Application.Features.ProductImageFile.Queries.GetProductImage;
using Microsoft.AspNetCore.Authorization;

namespace eCommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);// Return the response from the mediator
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute]GetByIdProductQueryRequest getByIdProductQueryRequest)  
        {
            GetByIdProductQueryResponse response= await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
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
        public async Task<IActionResult> Put([FromBody ]UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return NoContent(); // Return 204 No Content on successful update
        }
 
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest) 
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
             
            return NoContent(); // 204 
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery]UploadProductImageCommandRequest uploadProductImageCommandRequest){
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
   
            return Ok(); // Return 200 OK after uploading files
        }


        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetProductImages([FromRoute]GetProductImageQueryRequest getProductImageQueryRequest)
        {
            List<GetProductImageQueryResponse> response = await _mediator.Send(getProductImageQueryRequest);
            return Ok(response);
        }

        [HttpDelete("[action]/{productId}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute]RemoveProductImageCommandRequest  removeProductImageCommandRequest, [FromQuery]string imageId)
        {
            removeProductImageCommandRequest.imageId= imageId; 
            RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
    }
}
