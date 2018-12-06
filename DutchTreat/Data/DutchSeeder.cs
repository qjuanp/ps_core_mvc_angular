using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IHostingEnvironment _hosting;

        public DutchSeeder(
            DutchContext context,
            IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        public async Task Seed()
        {
            if (_context.Products.Any()) return;

            var products = await ListExampleProducts();
            await _context
                .Products
                .AddRangeAsync(products);

            await AddExampleOrder(products.FirstOrDefault());

            await _context.SaveChangesAsync();
        }

        private async Task<IEnumerable<Product>> ListExampleProducts()
        {
            var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
            var json = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
        }

        private async Task AddExampleOrder(Product product)
        {
            if (!await _context.Orders.AnyAsync(o => o.Id == 1))
            {
                await _context.Orders.AddAsync(new Order
                {
                    OrderDate = new DateTime(),
                    OrderNumber = "1",
                    Items = new List<OrderItem>() {
                        new OrderItem {
                            Product = product,
                            Quantity = 5,
                            UnitPrice = product.Price
                        }
                    }
                });
            }
        }
    }
}