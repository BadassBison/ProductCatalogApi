using System;

namespace Product_Catalog_Api.Dtos
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
