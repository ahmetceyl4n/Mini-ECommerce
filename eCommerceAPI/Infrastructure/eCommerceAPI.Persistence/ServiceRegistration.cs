using Microsoft.EntityFrameworkCore;
using eCommerceAPI.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Persistence.Repositories;
using eCommerceAPI.Domain.Entities.Identity;
using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Persistence.Services;
using eCommerceAPI.Application.Abstractions.Services.Authentications;

namespace eCommerceAPI.Persistence
{
    public static class ServiceRegistration                 //IoC Container' ne eklemek istiyorsak buraya yazıyoruz. IoC container bağlantıları sağlar yani IProduct çağrıldığında Product döndür gibi komutlar yazarız
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<eCommerceAPIDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 3; // Şifre uzunluğunu 3 karakter olarak ayarlıyoruz
                options.Password.RequireNonAlphanumeric = false; // Özel karakter gereksinimini kaldırıyoruz
                options.Password.RequireDigit = false; // Rakam gereksinimini kaldırıyoruz
            }).AddEntityFrameworkStores<eCommerceAPIDbContext>(); //Identity ile ilgili işlemleri yapabilmek için Identity'yi ekliyoruz. AppUser ve AppRole sınıflarını kullanarak Identity'yi yapılandırıyoruz. 

            services.AddScoped<ICustomerReadRepositories, CustomerReadRepository>();    //DbContext Scoped kullandığı için burada scoped kullandık. İlerde hata almamak için.
            services.AddScoped<ICustomerWriteRepositories, CustomerWriteRepository>();
            services.AddScoped<IOrderReadRepositories, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepositories, OrderWriteRepository>();
            services.AddScoped<IProductReadRepositories, ProductReadRepository>();
            services.AddScoped<IProductWriteRepositories, ProductWriteRepository>();
            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();
            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IUserService, UserService>(); 
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();
            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();

        }
    }
}
