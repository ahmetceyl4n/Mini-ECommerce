using eTicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTicaretAPI.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<eTicaretAPIDbContext>         //visual Studio değil de dotnet cli gibi başka yerde çalışırsak dbcontext bağlantılarını görebilmesi için bu sınıf kullanılır
    {
        public eTicaretAPIDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<eTicaretAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
