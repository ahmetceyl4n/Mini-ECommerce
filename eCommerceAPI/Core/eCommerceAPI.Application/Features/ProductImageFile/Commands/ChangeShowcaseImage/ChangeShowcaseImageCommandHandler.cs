using eCommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.ProductImageFile.Commands.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table.Include(p => p.Products).SelectMany(p=> p.Products , (pif, p) => new
            {
                pif,
                p
            });

            var data = await query.FirstOrDefaultAsync(p => p.pif.Showcase && p.p.ID == Guid.Parse(request.ProductId));

            if (data != null)
            {
                data.pif.Showcase = false;
            }

            var image = await query.FirstOrDefaultAsync(p => p.pif.ID == Guid.Parse(request.ImageId));
            if (image != null)
                image.pif.Showcase = true;

            await _productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
