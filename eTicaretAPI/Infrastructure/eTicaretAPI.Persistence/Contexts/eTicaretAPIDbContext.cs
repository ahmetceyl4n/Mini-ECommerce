using eTicaretAPI.Domain.Entities;
using eTicaretAPI.Domain.Entities.Common;
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)       //Interceptor işlemi için bu metodu override ediyoruz. Interceptor veriyi database'e kaydedecekken araya girer ve eklemeler yapmamızı sağlar.
                                                                                                        //Yani örneğin bir ürün eklendiğinde otomatik olarak ekleme tarihi oluşturacak 
        {
            var entries = ChangeTracker.Entries<BaseEntity>();          //ChangeTracker, veritabanındaki değişiklikleri takip eder. BaseEntity sınıfından türetilen tüm entity'leri alır
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:                             //Eğer entity ekleniyorsa
                        entry.Entity.CreatedDate = DateTime.UtcNow;     //O anki tarih ve saati CreatedDate alanına atar
                        break;
                    case EntityState.Modified:                          //Eğer entity güncelleniyorsa
                        entry.Entity.UpdatedDate = DateTime.UtcNow;     //O anki tarih ve saati UpdatedDate alanına atar
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }                                              
    }
}
