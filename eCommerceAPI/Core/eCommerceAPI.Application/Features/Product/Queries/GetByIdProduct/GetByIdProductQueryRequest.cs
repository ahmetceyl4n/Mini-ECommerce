using eCommerceAPI.Application.Features.Product.Commands.UpdateProduct;
using MediatR;

namespace eCommerceAPI.Application.Features.Product.Queries.GetByIdProduct;

public class GetByIdProductQueryRequest : IRequest<GetByIdProductQueryResponse>
{
    public string Id { get; set; }
}