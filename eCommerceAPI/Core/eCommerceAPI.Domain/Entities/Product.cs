using eCommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }

        public ICollection<Order> Orders { get; set; }       //Product'ın Order ile bire çok ilişkisi olduğu anlamına gelir. Karşılıklı tanımlanırsa çoka çok olur
        public ICollection<ProductImageFile> productImageFiles { get; set; }    // Product ile ProductImageFile arasında bire çok ilişkisi vardır, yani bir ürünün birden fazla resim dosyası olabilir

        public ICollection<BasketItem> BasketItems { get; set; } // Product ile BasketItem arasında bire çok ilişkisi vardır, yani bir ürün birden fazla sepet öğesinde bulunabilir
    }
}
