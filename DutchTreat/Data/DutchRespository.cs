using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Data
{
    class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(
            DutchContext context,
            ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Product>> GetAll()
        {
            _logger.LogInformation("GetAll was called");
            return await _context
                .Products
                .OrderBy(p => p.Title)
                .ToListAsync();
        }
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