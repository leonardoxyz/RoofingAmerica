using Microsoft.EntityFrameworkCore;
using RoofingAmerica.Domain.Core.Data;
using RoofingAmerica.Domain.Models;
using RoofingAmerica.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoofingAmerica.Infrastructure.Repository
{
    public class SaleRepository : IRepository<Sale, Guid>
    {
        private readonly DataContext _context;

        public SaleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Sale entity)
        {
            await _context.Sales.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
            }
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales.ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(Guid id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                return sale;
            } 
            else
            {
                return null;
            }
        }

        public async Task UpdateAsync(Sale entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
