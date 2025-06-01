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
    public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepositories
    {
        public CustomerWriteRepository(eTicaretAPIDbContext context) : base(context)
        {
        }
    }
}
