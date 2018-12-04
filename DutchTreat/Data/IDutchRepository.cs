using System.Collections.Generic;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        Task<IEnumerable<Product>> GetAll();

        Task<IEnumerable<Product>> GetByCategory(string category);

        Task<bool> SaveAll();
    }
}