using System.Collections.Generic;
using System.Threading.Tasks;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Services
{
  public interface IProductService
  {
    Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
    Task<ProductEntity> GetProductByIdAsync(int id);
    // Task<ProductEntity> GetProductByNameAsync(string name);
    // Task<bool> CheckForProductAsync(string name);
  }
}
