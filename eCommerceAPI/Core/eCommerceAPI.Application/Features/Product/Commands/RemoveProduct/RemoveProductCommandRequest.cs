using eCommerceAPI.Application.Features.Product.Commands.CreateProduct;
using MediatR;

namespace eCommerceAPI.Application.Features.Product.Commands.RemoveProduct;

public class RemoveProductCommandRequest : IRequest<RemoveProductCommandResponse>
{
    public string Id { get; set; }
}