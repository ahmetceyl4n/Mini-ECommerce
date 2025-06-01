using eTicaretAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTicaretAPI.Persistence.Contexts
{
    public class eTicaretAPIDbContext : DbContext                                               //Veritabanının kod karşılığını bu sınıfa yazarız
    {
        public eTicaretAPIDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }                    //Product türünde Products isminde tablo oluşturur
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
