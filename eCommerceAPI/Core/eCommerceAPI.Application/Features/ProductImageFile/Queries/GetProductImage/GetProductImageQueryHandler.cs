using eCommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eCommerceAPI.Application.Features.ProductImageFile.Queries.GetProductImage;

public class GetProductImageQueryHandler :  IRequestHandler<GetProductImageQueryRequest, List<GetProductImageQueryResponse>>
{
    private readonly IProductReadRepositories _productReadRepositories;
    private readonly IConfiguration _configuration;

    public GetProductImageQueryHandler(IProductReadRepositories productReadRepositories, IConfiguration configuration)
    {
        _productReadRepositories = productReadRepositories;
        _configuration = configuration;
    }

    public async Task<List<GetProductImageQueryResponse>> Handle(GetProductImageQueryRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product? product = await _productReadRepositories.Table.Include(p => p.productImageFiles).FirstOrDefaultAsync(p => p.ID == Guid.Parse(request.Id)); // Ürün ID'sine göre ürünü alıyoruz

        return product.productImageFiles.Select(p => new GetProductImageQueryResponse
        {
            Path = $"{_configuration["BaseStorageUrl"]}/{p.FilePath}",
            FileName = p.FileName,
            Id = p.ID
        }).ToList()
        ;
    }
}   