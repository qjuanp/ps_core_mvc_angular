using System.Collections.Generic;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<IEnumerable<Product>> GetProductsByCategory(string category);

        Task<IEnumerable<Order>> GetAllOrders(bool includeItems);
        
        Task<Order> GetOrderById(int id);
        
        Task AddEntity(object model);

        Task<bool> SaveAll();
    }
}