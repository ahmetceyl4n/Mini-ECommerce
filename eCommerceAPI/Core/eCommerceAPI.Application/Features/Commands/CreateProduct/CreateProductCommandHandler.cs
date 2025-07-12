using eCommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        readonly IProductWriteRepositories _productWriteRepositories;
        
        public CreateProductCommandHandler(IProductWriteRepositories productWriteRepositories)
        {
            _productWriteRepositories = productWriteRepositories;
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
            
            return new(); // Return a new response object
        }
    }
}
