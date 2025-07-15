using MediatR;

namespace eCommerceAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandRequest : IRequest<RemoveProductImageCommandResponse>
{
    public string productId { get; set; }
    public string? imageId { get; set; }
}