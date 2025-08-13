using eCommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Application.Features.ProductImageFile.Commands.RemoveProductImage;

public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
{
    private readonly IProductReadRepositories _productReadRepositories;
    private readonly IProductWriteRepositories _productWriteRepositories;

    public RemoveProductImageCommandHandler(IProductReadRepositories productReadRepositories, IProductWriteRepositories productWriteRepositories)
    {
        _productReadRepositories = productReadRepositories;
        _productWriteRepositories = productWriteRepositories;
    }

    public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product? product = await _productReadRepositories.Table.Include(p => p.productImageFiles).FirstOrDefaultAsync(p => p.ID == Guid.Parse(request.productId)); // Ürün ID'sine göre ürünü alıyoruz

        Domain.Entities.ProductImageFile? productImageFile = product?.productImageFiles.FirstOrDefault(p => p.ID == Guid.Parse(request.imageId));

        if (productImageFile != null)
            product?.productImageFiles.Remove(productImageFile);

        await _productWriteRepositories.SaveAsync(); 

        return new RemoveProductImageCommandResponse();
    }
}