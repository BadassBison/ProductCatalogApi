using AutoMapper;
using Product_Catalog_Api.Models;
using Product_Catalog_Api.Dtos;

namespace Product_Catalog_Api.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            this.CreateMap<ProductEntity, Product>().ReverseMap()
                .ForMember(m => m.ProductId, opt => opt.Ignore());
        }
    }
}
