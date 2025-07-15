using eCommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace eCommerceAPI.Application.Features.Commands.Product.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    private readonly IProductReadRepositories _productReadRepositories;
    private readonly IProductWriteRepositories _productWriteRepositories;

    public UpdateProductCommandHandler(IProductReadRepositories productReadRepositories, IProductWriteRepositories productWriteRepositories)
    {
        _productReadRepositories = productReadRepositories;
        _productWriteRepositories = productWriteRepositories;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = await _productReadRepositories.GetByIdAsync(request.id);
        
        product.Name = request.Name;
        product.Stock = request.Stock;
        product.Price = request.Price;
        
        await _productWriteRepositories.SaveAsync();
        return new UpdateProductCommandResponse();
    }
}