using eCommerceAPI.Application.Repositories;
using MediatR;

namespace eCommerceAPI.Application.Features.Product.Queries.GetByIdProduct;

public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
{
    private readonly IProductReadRepositories  _productReadRepositories;
    
    public GetByIdProductQueryHandler(IProductReadRepositories  productReadRepositories)
    {
        _productReadRepositories = productReadRepositories;
    }
 
    public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = await _productReadRepositories.GetByIdAsync(request.Id, false);
        
        return new GetByIdProductQueryResponse()
        {
            Name = product.Name,
            Stock = product.Stock,
            Price = product.Price
        };
    }
}