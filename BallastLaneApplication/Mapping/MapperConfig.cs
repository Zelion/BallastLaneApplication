using AutoMapper;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Mapping
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // Product
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
