using eCommerceAPI.Application.Abstractions.Hubs;
using eCommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        readonly IProductWriteRepositories _productWriteRepositories;
        readonly IProductHubServices _productHubServices;

        public CreateProductCommandHandler(IProductWriteRepositories productWriteRepositories, IProductHubServices productHubServices)
        {
            _productWriteRepositories = productWriteRepositories;
            _productHubServices = productHubServices;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productWriteRepositories.AddAsync(new()
            {
                Name = request.Name,
                Stock = request.Stock,
                Price = request.Price
            });     // Create a new product entity
            
            await _productWriteRepositories.SaveAsync();
            await _productHubServices.ProductAddedMessageAsync($"The product named {request.Name} has been added");


            return new(); // Return a new response object
        }
    }
}
