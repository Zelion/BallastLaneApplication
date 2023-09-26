using AutoMapper;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;

namespace BallastLaneAuth.Mapping
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // User
            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
