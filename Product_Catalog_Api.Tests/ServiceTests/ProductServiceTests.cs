using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;

using Product_Catalog_Api.Database;
using Product_Catalog_Api.Services;
using Product_Catalog_Api.Models;
using Product_Catalog_Api.Dtos;

namespace Kairos_Product_Api.Tests
{
  public class GetProductRepositoryTests
  {
    string Entities = "products";

    [Fact]
    public async Task GetAllProducts_WhenThreeAreAdded_ReturnsThree()
    {
      // Arrange
      var connectionStringBuild =
          new SqliteConnectionStringBuilder { DataSource = ":memory:" };
      var connection = new SqliteConnection(connectionStringBuild.ToString());

      var options = new DbContextOptionsBuilder<ProductCatalogApiDbContext>()
          .UseSqlite(connection)
          .Options;

      using (var context = new ProductCatalogApiDbContext(options))
      {
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        context.AddRange(new List<ProductEntity>
        {
          new ProductEntity()
          {
            Name = "Test product 1",
            Quantity = 0,
            Cost = 24.89,
            Price = 44.93,
            Description = "test description",
            ManufacturerId = 1,
            CreatedDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now
          },
          new ProductEntity()
          {
            Name = "Test product 2",
            Quantity = 0,
            Cost = 47.54,
            Price = 62.49,
            Description = "test description",
            ManufacturerId = 2,
            CreatedDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now
          },
          new ProductEntity()
          {
            Name = "Test product 3",
            Quantity = 0,
            Cost = 30.34,
            Price = 49.95,
            Description = "test description",
            ManufacturerId = 3,
            CreatedDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now
          }
        }.ToArray());
        context.SaveChanges();
      }

      using (var context = new ProductCatalogApiDbContext(options))
      {
        var service = new ProductService(context);

        // Act
        var products = await service.GetAllProductsAsync();

        // Assertion
        var expectedCount = 3;
        var msg = $"There are {expectedCount} {Entities} in the database";
        products.Should().HaveCount(expectedCount, because: msg);
      }
    }

    [Fact]
    public async Task AddProductAsync_AddsProduct_WhenSuccessful()
    {
      // Arrange
      var connectionStringBuild =
          new SqliteConnectionStringBuilder { DataSource = ":memory:" };
      var connection = new SqliteConnection(connectionStringBuild.ToString());

      var options = new DbContextOptionsBuilder<ProductCatalogApiDbContext>()
          .UseSqlite(connection)
          .Options;

      using (var context = new ProductCatalogApiDbContext(options))
      {
        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();

        var product = new ProductEntity()
        {
          Name = "Test product 1",
          Quantity = 0,
          Cost = 24.89,
          Price = 44.93,
          Description = "test description",
          ManufacturerId = 1,
          CreatedDate = DateTime.Now,
          LastUpdatedDate = DateTime.Now
        };
        var service = new ProductService(context);

        // Act 1
        var entity = await service.AddProductAsync(product);

        // Assertion 1
        var expected = "Test product 1";
        var msg = $"Product {entity.Name} was returned from add method";
        entity.Name.Should().Be(expected, because: msg);
      }

      using (var context = new ProductCatalogApiDbContext(options))
      {
        var products = context.Products;

        // Act 2 - Check if item is is DB
        var id = 1;
        var entity = products.First(p => p.ProductId == id);

        // Assertion 2
        var expected = "Test product 1";
        var msg = $"Product {entity.Name} exists in database";
        entity.Name.Should().Be(expected, because: msg);
      }
    }

    [Fact]
    public async Task UpdateProduct_UpdatesAProductInDB_StoresNewLog_ReturnsObject()
    {
      // Arrange
      var connectionStringBuild =
        new SqliteConnectionStringBuilder { DataSource = ":memory:" };
      var connection = new SqliteConnection(connectionStringBuild.ToString());

      var options = new DbContextOptionsBuilder<ProductCatalogApiDbContext>()
        .UseSqlite(connection)
        .Options;

      var entity = new ProductEntity()
      {
        Name = "Test product 1",
        Quantity = 0,
        Cost = 24.89,
        Price = 44.93,
        Description = "test description",
        ManufacturerId = 1,
        CreatedDate = DateTime.Now,
        LastUpdatedDate = DateTime.Now
      };

      using (var context = new ProductCatalogApiDbContext(options))
      {
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        context.Products.Add(entity);
        context.SaveChanges();
      }

      using (var context = new ProductCatalogApiDbContext(options))
      {
        var updatedProduct = new Product()
        {
          Name = "Test product 1",
          Quantity = 0,
          Cost = 24.89,
          Price = 60,
          Description = "test description",
          ManufacturerId = 1
        };

        var service = new ProductService(context);

        // Act 1
        var updatedEntity = await service.UpdateProductAsync(entity, updatedProduct);

        // Assertion 1
        var expectedPrice = 60;
        var msg = $"Product {updatedEntity.Name} has an updated price of {expectedPrice}";
        updatedEntity.Price.Should().Be(expectedPrice, because: msg);
      }

      using (var context = new ProductCatalogApiDbContext(options))
      {
        // Act 2
        var id = 1;
        var fetchedPriceLog = context.PriceLogs.First(p => p.ProductId == id);

        // Assertion 2
        var expectedPrice = 60;
        fetchedPriceLog.Price.Should().Be(expectedPrice);
      }
    }

    [Fact]
    public async Task RemoveProduct_RemovesProduct_ReturnsObject()
    {
      // Arrange
      var connectionStringBuild =
        new SqliteConnectionStringBuilder { DataSource = ":memory:" };
      var connection = new SqliteConnection(connectionStringBuild.ToString());

      var options = new DbContextOptionsBuilder<ProductCatalogApiDbContext>()
          .UseSqlite(connection)
          .Options;

      using (var context = new ProductCatalogApiDbContext(options))
      {
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        context.Products.Add(new ProductEntity()
        {
          Name = "Test product 1",
          Quantity = 0,
          Cost = 24.89,
          Price = 44.93,
          Description = "test description",
          ManufacturerId = 1,
          CreatedDate = DateTime.Now,
          LastUpdatedDate = DateTime.Now
        });
        context.SaveChanges();
      }

      using (var context = new ProductCatalogApiDbContext(options))
      {
        var service = new ProductService(context);
        string name = "Test product 1";
        var product = context.Products.First(p => p.Name == name);

        // Act
        var entity = await service.RemoveProductAsync(product);

        // Assertion 1
        var expectedName = "Test product 1";
        var msg = $"Product {entity.Name} was returned from the remove method";
        entity.Name.Should().Be(expectedName, because: msg);
      }

      using (var context = new ProductCatalogApiDbContext(options))
      {
        // Assertion 2
        var expected = 0;
        var msg = $"There are no entities in the database";
        context.Products.Should().HaveCount(expected, because: msg);
      }
    }
  }
}
