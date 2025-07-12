using eCommerceAPI.Application.Features.Queries.GetAllProduct;
using eCommerceAPI.Application.Repositories;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Queries.GetAllProduct
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
            var totalCount = _productReadRepositories.GetAll(false).Count();
            var products = _productReadRepositories.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Select(p => new
            {
                p.ID,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            }).ToList();

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}