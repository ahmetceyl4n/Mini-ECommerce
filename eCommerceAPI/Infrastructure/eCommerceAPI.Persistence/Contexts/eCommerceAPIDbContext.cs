﻿using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Entities.Common;
using eCommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Contexts
{
    public class eCommerceAPIDbContext : IdentityDbContext<AppUser,AppRole, string>                                              //Veritabanının kod karşılığını bu sınıfa yazarız
    {
        public eCommerceAPIDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }                    //Product türünde Products isminde tablo oluşturur
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            builder.Entity<Order>()
                .HasKey(b => b.ID);

            builder.Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(b => b.Basket)
                .HasForeignKey<Order>(p => p.ID);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)       //Interceptor işlemi için bu metodu override ediyoruz. Interceptor veriyi database'e kaydedecekken araya girer ve eklemeler yapmamızı sağlar.
                                                                                                        //Yani örneğin bir ürün eklendiğinde otomatik olarak ekleme tarihi oluşturacak 
        {
            var entries = ChangeTracker.Entries<BaseEntity>();          //ChangeTracker, veritabanındaki değişiklikleri takip eder. BaseEntity sınıfından türetilen tüm entity'leri alır
            foreach (var entry in entries)
            {
                _ = entry.State switch
                {
                    EntityState.Added => entry.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => entry.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }
            return base.SaveChangesAsync(cancellationToken);
        }                                              
    }
}
