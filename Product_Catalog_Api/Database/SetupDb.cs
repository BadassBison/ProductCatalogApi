using System;
using System.Collections.Generic;
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

      // context.Database
      context.Database.Migrate();

      if(!context.Products.Any())
      {
        System.Console.WriteLine("Seeding data...");

        context.Products.AddRange(
          new List<ProductEntity>()
          {
            new ProductEntity
            {
              Name = "Hammer",
              Price = 5.97,
              Cost = 3.46,
              Quantity = 28,
              Description = "Used for hammering things",
              ManufacturerId = 101,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            },
            new ProductEntity
            {
              Name = "Screwdriver Set",
              Price = 49.97,
              Cost = 31.82,
              Quantity = 19,
              Description = "Used for turning screws",
              ManufacturerId = 101,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            },
            new ProductEntity
            {
              Name = "Pliers",
              Price = 5.39,
              Cost = 3.15,
              Quantity = 22,
              Description = "Used for grasping things",
              ManufacturerId = 101,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            },
            new ProductEntity
            {
              Name = "Shovel",
              Price = 7.98,
              Cost = 4.80,
              Quantity = 6,
              Description = "Used for shoveling things",
              ManufacturerId = 102,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            },
            new ProductEntity
            {
              Name = "Axe",
              Price = 14.99,
              Cost = 7.43,
              Quantity = 6,
              Description = "Used for chopping things",
              ManufacturerId = 102,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            },
            new ProductEntity
            {
              Name = "Drill",
              Price = 49.99,
              Cost = 32.75,
              Quantity = 11,
              Description = "Used for drilling things",
              ManufacturerId = 103,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            },
          }.ToArray()
        );
        context.SaveChanges();

        context.PriceLogs.AddRange(
          new List<PriceLog>
          {
            new PriceLog
            {
              ProductId = 1,
              Price = 5.97,
              UpdatedDate = DateTime.Now
            },
            new PriceLog
            {
              ProductId = 2,
              Price = 49.97,
              UpdatedDate = DateTime.Now
            },
            new PriceLog
            {
              ProductId = 3,
              Price = 5.39,
              UpdatedDate = DateTime.Now
            },
            new PriceLog
            {
              ProductId = 4,
              Price = 7.98,
              UpdatedDate = DateTime.Now
            },
            new PriceLog
            {
              ProductId = 5,
              Price = 14.99,
              UpdatedDate = DateTime.Now
            },
            new PriceLog
            {
              ProductId = 6,
              Price = 49.99,
              UpdatedDate = DateTime.Now
            }
          }.ToArray()
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
