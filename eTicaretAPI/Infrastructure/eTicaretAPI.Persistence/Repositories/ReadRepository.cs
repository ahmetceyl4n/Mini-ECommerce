using eTicaretAPI.Application.Repositories;
using eTicaretAPI.Domain.Entities.Common;
using eTicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eTicaretAPI.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly eTicaretAPIDbContext _context;  //DbContext'e erişimi sağlıyor

        public ReadRepository(eTicaretAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();      //DbContext'te T type olmadığı için (Product,costumer falan var) Set<T> kullanırız

        public IQueryable<T> GetAll(bool tracking = true)
        //=> Table;         //Tracking kontrolü öncesi
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        //=> Table.Where(method);       
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        // => await Table.FirstOrDefaultAsync(method);
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }
        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        // => await Table.FirstOrDefaultAsync(data => data.ID == Guid.Parse(id));       //Daha uzun yolu
        //=> await Table.FindAsync(Guid.Parse(id)) ;
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.ID == Guid.Parse(id));  //Quaryable'da find methodu yok 
        }
    }
}
        