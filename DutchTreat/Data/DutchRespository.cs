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
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            _logger.LogInformation("GetAll was called");
            return await _context
                .Products
                .OrderBy(p => p.Title)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByCategory(string category) =>
            await _context
                .Products
                .Where(p => p.Category == category)
                .OrderBy(p => p.Title)
                .ToListAsync();

        public async Task<IEnumerable<Order>> GetAllOrders() =>
            await _context
                .Orders
                .Include(o => o.Items)
                    .ThenInclude(o => o.Product)
                .ToListAsync();

        public async Task<Order> GetOrderById(int id) =>
            await _context
                .Orders
                .Include(o => o.Items)
                    .ThenInclude(o => o.Product)
                .SingleOrDefaultAsync(o => o.Id == id);

        public async Task AddEntity(object model) =>
            await _context.AddAsync(model);

        public async Task<bool> SaveAll() =>
            await _context.SaveChangesAsync() > 0;
    }
}