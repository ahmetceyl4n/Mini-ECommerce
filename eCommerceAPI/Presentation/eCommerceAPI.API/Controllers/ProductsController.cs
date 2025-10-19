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
using eCommerceAPI.Application.Features.ProductImageFile.Commands.ChangeShowcaseImage;

namespace eCommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
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
        [Authorize(AuthenticationSchemes = "Admin")]
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
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Put([FromBody ]UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return NoContent(); // Return 204 No Content on successful update
        }
 
        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest) 
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
             
            return NoContent(); // 204 
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Upload([FromQuery]UploadProductImageCommandRequest uploadProductImageCommandRequest){
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
   
            return Ok(); // Return 200 OK after uploading files
        }


        [HttpGet("[action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetProductImages([FromRoute]GetProductImageQueryRequest getProductImageQueryRequest)
        {
            List<GetProductImageQueryResponse> response = await _mediator.Send(getProductImageQueryRequest);
            return Ok(response);
        }

        [HttpDelete("[action]/{productId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> DeleteProductImage([FromRoute]RemoveProductImageCommandRequest  removeProductImageCommandRequest, [FromQuery]string imageId)
        {
            removeProductImageCommandRequest.imageId= imageId; 
            RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }

        [HttpPut("[action]/{imageId}/{productId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery]ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
            ChangeShowcaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok();
        }

    }
}
