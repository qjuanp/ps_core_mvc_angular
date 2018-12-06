using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    // IdentityDbContext -> Use EF as user source for the app
    //  - Generic parameter -> User information definition
    public class DutchContext : IdentityDbContext<StoredUser>
    {

        public DutchContext(DbContextOptions<DutchContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}