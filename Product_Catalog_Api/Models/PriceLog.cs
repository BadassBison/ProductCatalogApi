using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog_Api.Models
{
  [Table("pricelog")]
  public class PriceLog
  {
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    [Column("productId")]
    public int ProductId { get; set; }
    [Column("price")]
    public double Price { get; set; }
    [Column("updatedDate")]
    public DateTime UpdatedDate { get; set; }
  }
}
