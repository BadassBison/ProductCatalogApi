using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product_Catalog_Api.Database;
using Product_Catalog_Api.Models;
using Product_Catalog_Api.Dtos;

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

    public List<ProductEntity> GetProductsByName(string name)
    {
      var searchTerm = "%" + name + "%";
      return _context.Products
        .FromSqlInterpolated($"SELECT * FROM dbo.products WHERE RTRIM(name) LIKE {searchTerm}")
        .ToList();
    }

    public async Task<bool> CheckForProductAsync(string name)
    {
      return await _context.Products.FirstOrDefaultAsync(p => p.Name == name) != null;
    }

    public async Task<ProductEntity> AddProductAsync(ProductEntity entity)
    {
      var exists = await CheckForProductAsync(entity.Name);
      if (exists) throw new InvalidOperationException($"{entity.Name} already exists");

      _context.Products.Add(entity);

      if (await _context.SaveChangesAsync() > 0) return entity;
      else throw new Exception("Failed to save to the database");
    }

    public async Task<ProductEntity> UpdateProductAsync(ProductEntity entity, Product model)
    {
      entity.Name              = model.Name == null              ? entity.Name           : model.Name;
      entity.Quantity          = model.Quantity == null          ? entity.Quantity       : (int)model.Quantity;
      entity.Price             = model.Price == null             ? entity.Price          : (double)model.Price;
      entity.Cost              = model.Cost == null              ? entity.Cost           : (double)model.Cost;
      entity.Description       = model.Description == null       ? entity.Description    : model.Description;
      entity.ManufacturerId    = model.ManufacturerId == null    ? entity.ManufacturerId : (int)model.ManufacturerId;
      entity.LastUpdatedDate   = DateTime.Now;

      await UpdatePriceLog(entity);
      if(await _context.SaveChangesAsync() > 0) return entity;
      else throw new Exception($"Failed to update {entity.Name} to the database");
    }

    public async Task<ProductEntity> RemoveProductAsync(ProductEntity product)
    {
      _context.Products.Remove(product);

      if(await _context.SaveChangesAsync() > 0) return product;
      else throw new Exception($"Failed to remove {product.Name} from the database");
    }

    public async Task UpdatePriceLog(ProductEntity product)
    {
      var newPriceLog = new PriceLog
      {
        ProductId = product.ProductId,
        Price = product.Price,
        UpdatedDate = DateTime.Now
      };

      await _context.PriceLogs.AddAsync(newPriceLog);
    }
  }


}
