using System;
using Microsoft.EntityFrameworkCore;

namespace Product_Catalog_Api.Database
{
  public class ProductCatalogApiContext : DbContext
  {
    public DbSet<Product> Products { get; set; }
    public ProductCatalogApiContext(DbContextOptions<ProductCatalogApiContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {

    }
  }
}
