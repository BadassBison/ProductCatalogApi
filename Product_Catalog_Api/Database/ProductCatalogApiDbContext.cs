using System;
using Microsoft.EntityFrameworkCore;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Database
{
  public class ProductCatalogApiDbContext : DbContext
  {
    public DbSet<ProductEntity> Products { get; set; }
    public ProductCatalogApiDbContext(DbContextOptions<ProductCatalogApiDbContext> options) : base(options) {}

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //   var entity = builder.Entity<ProductEntity>();
    // }
  }
}
