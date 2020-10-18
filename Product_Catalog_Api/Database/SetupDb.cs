using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Database
{
  public static class SetupDb
  {
    public static void SetupConfig(IApplicationBuilder app)
    {
      using (var serviceScope = app.ApplicationServices.CreateScope())
      {
        seedDb(serviceScope.ServiceProvider.GetService<ProductCatalogApiDbContext>());
      }
    }

    public static void seedDb(ProductCatalogApiDbContext context)
    {
      System.Console.WriteLine("Appling Migrations...");

      context.Database.Migrate();

      if(!context.Products.Any())
      {
        System.Console.WriteLine("Seeding data...");

        context.AddRange(
          Enumerable.Range(1, 7).Select(index => new ProductEntity
            {
              Name = $"Product {index}",
              Price = 11,
              Cost = 20,
              Quantity = 0,
              Description = "Description for the product",
              ManufacturerId = index + 5,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            }
          ).ToArray()
        );
        context.SaveChanges();
      }
      else
      {
        System.Console.WriteLine("Already had data, not seeding data...");
      }
    }
  }
}
