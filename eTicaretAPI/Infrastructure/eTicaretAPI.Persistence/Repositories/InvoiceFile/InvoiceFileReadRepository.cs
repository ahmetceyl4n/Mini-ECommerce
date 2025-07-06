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
    public class InvoiceFileReadRepository : ReadRepository<InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(eTicaretAPIDbContext context) : base(context)
        {
        }
    }
}
