using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Product_Catalog_Api.Dtos;

namespace Product_Catalog_Api.Models
{
  [Table("products")]
  public class ProductEntity
  {
    [Key]
    [Column("productId")]
    public int ProductId { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("price")]
    public double Price { get; set; }
    [Column("cost")]
    public double Cost { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("manufacturerId")]
    public int ManufacturerId { get; set; }
    [Column("createdDate")]
    public DateTime CreatedDate { get; set; }
    [Column("lastUpdatedDate")]
    public DateTime LastUpdatedDate { get; set; }

    public static implicit operator ProductEntity(Product dto)
    {
      return new ProductEntity
      {
        ProductId = dto.ProductId,
        Name = dto.Name,
        Price = dto.Price,
        Cost = dto.Cost,
        Quantity = dto.Quantity,
        Description = dto.Description,
        ManufacturerId = dto.ManufacturerId,
        CreatedDate = DateTime.Now,
        LastUpdatedDate = DateTime.Now
      };
    }
  }
}
