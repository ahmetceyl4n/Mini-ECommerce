using Microsoft.EntityFrameworkCore;
using eTicaretAPI.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTicaretAPI.Application.Repositories;
using eTicaretAPI.Persistence.Repositories;

namespace eTicaretAPI.Persistence
{
    public static class ServiceRegistration                 //IoC Container' ne eklemek istiyorsak buraya yazıyoruz. IoC container bağlantıları sağlar yani IProduct çağrıldığında Product döndür gibi komutlar yazarız
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<eTicaretAPIDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
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
        }
    }
}
