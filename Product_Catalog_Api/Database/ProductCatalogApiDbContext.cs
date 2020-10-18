using System;
using Microsoft.EntityFrameworkCore;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Database
{
  public class ProductCatalogApiDbContext : DbContext
  {
    public ProductCatalogApiDbContext(DbContextOptions<ProductCatalogApiDbContext> options) : base(options) {}
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<PriceLog> PriceLogs { get; set; }
  }
}
