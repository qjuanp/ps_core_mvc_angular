using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly IConfiguration _configuration;
        private readonly UserManager<StoredUser> _userManager;

        public DutchSeeder(
            DutchContext context,
            IHostingEnvironment hosting,
            IConfiguration configuration,
            UserManager<StoredUser> userManager)
        {
            _context = context;
            _hosting = hosting;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            await _context.Database.EnsureCreatedAsync();

            StoredUser user = await _userManager.FindByEmailAsync(_configuration["Defaults:User:Email"]);

            if (user == null)
            {
                user = new StoredUser
                {
                    FirtsName = _configuration["Defaults:User:Name"],
                    LastName = _configuration["Defaults:User:LastName"],
                    Email = _configuration["Defaults:User:Email"],
                    UserName = _configuration["Defaults:User:Email"]
                };

                var result = await _userManager.CreateAsync(user, _configuration["Defaults:User:Password"]);

                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create new user in seeder");
            }

            if (_context.Products.Any()) return;

            var products = await ListExampleProducts();
            await _context
                .Products
                .AddRangeAsync(products);

            await AddExampleOrder(user, products.FirstOrDefault());

            await _context.SaveChangesAsync();
        }

        private async Task<IEnumerable<Product>> ListExampleProducts()
        {
            var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
            var json = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
        }

        private async Task AddExampleOrder(StoredUser user, Product product)
        {
            if (!await _context.Orders.AnyAsync(o => o.Id == 1))
            {
                await _context.Orders.AddAsync(new Order
                {
                    User = user,
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