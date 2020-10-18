using System;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Dtos
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int ManufacturerId { get; set; }

        public static implicit operator Product(ProductEntity entity)
        {
            return new Product
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Price = entity.Price,
                Cost = entity.Cost,
                Quantity = entity.Quantity,
                Description = entity.Description,
                ManufacturerId = entity.ManufacturerId
            };
        }
    }
}
