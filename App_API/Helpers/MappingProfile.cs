using App_API.Dtos.Users;
using App_API.Models;
using AutoMapper;

namespace App_API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.MiddleName} {src.LastName}"));
            CreateMap<UserAuthRequest, User>();
            CreateMap<UserAdminCreateRequest, User>();
        }
    }
}
