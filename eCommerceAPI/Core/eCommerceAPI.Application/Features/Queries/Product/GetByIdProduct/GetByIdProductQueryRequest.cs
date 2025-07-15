using eCommerceAPI.Application.Features.Commands.Product.UpdateProduct;
using MediatR;

namespace eCommerceAPI.Application.Features.Queries.Product.GetByIdProduct;

public class GetByIdProductQueryRequest : IRequest<GetByIdProductQueryResponse>
{
    public string Id { get; set; }
}