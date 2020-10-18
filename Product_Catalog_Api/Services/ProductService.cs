using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product_Catalog_Api.Database;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Services
{
  public class ProductService : IProductService
  {
    public readonly ProductCatalogApiDbContext _context;

    public ProductService(ProductCatalogApiDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
    {
      return await _context.Products.ToListAsync();
    }

    public async Task<ProductEntity> GetProductByIdAsync(int id)
    {
      return await _context.Products.FirstAsync(m => m.ProductId == id);
    }

    // public async Task<ProductEntity> GetProductByNameAsync(string name)
    // {
    //   return await _context.Products.FirstAsync(m => m.Name == name);
    // }

    // public async Task<bool> CheckForProductAsync(string name)
    // {
    //   return await _context.Products.FirstOrDefaultAsync(p => p.Name == name) != null;
    // }
  }
}
