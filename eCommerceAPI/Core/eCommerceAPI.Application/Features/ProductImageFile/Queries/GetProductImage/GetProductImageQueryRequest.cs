using MediatR;

namespace eCommerceAPI.Application.Features.ProductImageFile.Queries.GetProductImage;

public class GetProductImageQueryRequest : IRequest<List<GetProductImageQueryResponse>>
{
    public string Id { get; set; }
}