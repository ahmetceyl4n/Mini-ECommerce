﻿using eCommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }          //Order ın sadece bir customer ı olacağı için bu şekilde tanımlanır
        public string Description { get; set; }
        public string Address { get; set; }

        public ICollection<Product> Products { get; set; }       //Order'ın Product ile bire çok ilişkisi olduğu anlamına gelir. Karşılıklı tanımlanırsa çoka çok olur
        public Basket Basket { get; set; }
        public Customer Customer { get; set; }                   // Bir order ın sadece bir customer ı olacağı için bu şekilde tanımlanır
    }
}
