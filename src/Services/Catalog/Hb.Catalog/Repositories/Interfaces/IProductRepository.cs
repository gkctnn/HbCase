using Hb.Catalog.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hb.Catalog.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string id);
        //Task<IEnumerable<Product>> GetProductByName(string name);
        //Task<IEnumerable<Product>> GetProductByCategory(string categoryId);

        Task Create(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(string id);
    }
}
