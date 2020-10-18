using System.Collections.Generic;
using System.Threading.Tasks;
using Product_Catalog_Api.Dtos;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Services
{
  public interface IProductService
  {
    Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
    Task<ProductEntity> GetProductByIdAsync(int id);
    List<ProductEntity> GetProductsByName(string name);
    Task<ProductEntity> AddProductAsync(ProductEntity entity);
    Task<ProductEntity> UpdateProductAsync(ProductEntity entity, Product model);
    Task<ProductEntity> RemoveProductAsync(ProductEntity product);
  }
}
