
using eCommerceAPI.Application.Repositories;
using MediatR;

namespace eCommerceAPI.Application.Features.Commands.Product.RemoveProduct;

public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
{
    private readonly IProductWriteRepositories _productWriteRepositories;

    public RemoveProductCommandHandler(IProductWriteRepositories productWriteRepositories)
    {
        _productWriteRepositories = productWriteRepositories;
    }

    public  async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
    {
        await _productWriteRepositories.RemoveAsync(request.Id);
        await _productWriteRepositories.SaveAsync();

        return new RemoveProductCommandResponse();
    }
}