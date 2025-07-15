using eCommerceAPI.Application.Features.Commands.Product.CreateProduct;
using MediatR;

namespace eCommerceAPI.Application.Features.Commands.Product.RemoveProduct;

public class RemoveProductCommandRequest : IRequest<RemoveProductCommandResponse>
{
    public string Id { get; set; }
}