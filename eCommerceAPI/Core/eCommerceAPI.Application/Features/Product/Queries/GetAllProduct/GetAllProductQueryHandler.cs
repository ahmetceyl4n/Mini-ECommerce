using eCommerceAPI.Application.Repositories;

using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Product.Queries.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        private readonly IProductReadRepositories _productReadRepositories;

        public GetAllProductQueryHandler(IProductReadRepositories productReadRepositories)
        {
            _productReadRepositories = productReadRepositories;
        }
        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            var totalProductCount = _productReadRepositories.GetAll(false).Count();
            var products = _productReadRepositories.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                .Include(p => p.productImageFiles)
                .Select(p => new
            {
                p.ID,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
                p.productImageFiles
            }).ToList();

            return new()
            {
                Products = products,
                TotalProductCount = totalProductCount
            };
        }
    }
}