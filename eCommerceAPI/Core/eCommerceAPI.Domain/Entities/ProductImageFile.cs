using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class ProductImageFile : File
    {
        public ICollection<Product> Products { get; set; } // Product ile bire çok ilişkisi vardır, yani bir ürünün birden fazla resim dosyası olabilir
    }
}
