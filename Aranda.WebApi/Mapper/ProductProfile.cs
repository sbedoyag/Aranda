using Aranda.Core.Entities;
using Aranda.WebApi.Dtos;
using AutoMapper;

namespace Aranda.WebApi.Mapper
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<Product, ProductOutput>();
            CreateMap<ProductCreateInput, Product>();
        }
    }
}
