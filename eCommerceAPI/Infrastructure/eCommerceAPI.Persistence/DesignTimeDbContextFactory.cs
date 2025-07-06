using eCommerceAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<eCommerceAPIDbContext>         //visual Studio değil de dotnet cli gibi başka yerde çalışırsak dbcontext bağlantılarını görebilmesi için bu sınıf kullanılır
    {
        public eCommerceAPIDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<eCommerceAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
