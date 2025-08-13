using MediatR;
using Microsoft.AspNetCore.Http;

namespace eCommerceAPI.Application.Features.ProductImageFile.Commands.UploadProductImage;

public class UploadProductImageCommandRequest : IRequest<UploadProductImageCommandResponse>
{
    public string Id { get; set; }
    public IFormFileCollection? Files  { get; set; }
}