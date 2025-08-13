using eCommerceAPI.Application.Abstractions.Storage;
using eCommerceAPI.Application.Repositories;
using MediatR;

namespace eCommerceAPI.Application.Features.ProductImageFile.Commands.UploadProductImage;

public class
    UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest,
    UploadProductImageCommandResponse>
{
    private readonly IProductReadRepositories _productReadRepositories;
    private readonly IProductWriteRepositories _productWriteRepositories;
    private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    private readonly IStorageService _storageService;

    public UploadProductImageCommandHandler(IProductReadRepositories productReadRepositories,
        IProductWriteRepositories productWriteRepositories,
        IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService)
    {
        _productReadRepositories = productReadRepositories;
        _productWriteRepositories = productWriteRepositories;
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _storageService = storageService;
    } 

    public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request,
        CancellationToken cancellationToken)
    {
        List<(string fileName, string pathOrContainerName)> result =
            await _storageService.UploadAsync("photo-images", request.Files);
        Domain.Entities.Product product = await _productReadRepositories.GetByIdAsync(request.Id);

        await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new Domain.Entities.ProductImageFile
        {   
            FileName = r.fileName,
            FilePath = r.pathOrContainerName,
            Storage = _storageService.StorageName,
            Products = new List<Domain.Entities.Product>() { product } // Ürün ile ilişkilendiriyoruz
        }).ToList());

        await _productImageFileWriteRepository.SaveAsync();
        return new UploadProductImageCommandResponse();
    }
}