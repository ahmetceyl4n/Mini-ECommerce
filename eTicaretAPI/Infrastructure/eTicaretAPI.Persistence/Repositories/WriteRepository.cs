using eTicaretAPI.Application.Repositories;
using eTicaretAPI.Domain.Entities.Common;
using eTicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTicaretAPI.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly eTicaretAPIDbContext _context;

        public WriteRepository(eTicaretAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();     //Gerekl açıklamalar ReadRepository'te yazdım

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> datas)                  
        {
           await Table.AddRangeAsync(datas);
            return true;
        }

        public bool Remove(T model)
        {
            if (model == null)
                return false;

            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public bool RemoveRange(List<T> datas)
        {
            if (datas == null || !datas.Any())
                return false;

            Table.RemoveRange(datas);
            return true;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return false; // Geçersiz GUID

            T model = await Table.FirstOrDefaultAsync(data => data.ID == guidId);
            return Remove(model);
        }


        public bool Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();           
    }
}
