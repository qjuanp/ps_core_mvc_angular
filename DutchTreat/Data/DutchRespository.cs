using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;

        public DutchRepository(DutchContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAll() =>
            await _context
                .Products
                .OrderBy(p => p.Title)
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetByCategory(string category) =>
            await _context
                .Products
                .Where(p => p.Category == category)
                .OrderBy(p => p.Title)
                .ToListAsync();

        public async Task<bool> SaveAll() =>
            await _context.SaveChangesAsync() > 0;
    }
}