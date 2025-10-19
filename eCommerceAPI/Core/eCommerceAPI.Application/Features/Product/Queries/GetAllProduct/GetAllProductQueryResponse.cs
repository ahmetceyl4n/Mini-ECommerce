using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Product.Queries.GetAllProduct
{
    public class GetAllProductQueryResponse
    {
        public int TotalProductCount { get; set; }
        public object Products { get; set; }
    }
}
