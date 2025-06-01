using eTicaretAPI.Application.Repositories;
using eTicaretAPI.Domain.Entities;
using eTicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTicaretAPI.Persistence.Repositories
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepositories
    {
        public ProductWriteRepository(eTicaretAPIDbContext context) : base(context)
        {
        }
    }
}
