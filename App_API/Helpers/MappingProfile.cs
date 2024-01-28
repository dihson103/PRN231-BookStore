using App_API.Dtos.Authors;
using App_API.Dtos.Books;
using App_API.Dtos.Publishers;
using App_API.Dtos.Roles;
using App_API.Dtos.Users;
using App_API.Models;
using AutoMapper;

namespace App_API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
                //.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.MiddleName} {src.LastName}"));
            CreateMap<UserAuthRequest, User>();
            CreateMap<UserAdminCreateRequest, User>();

            CreateMap<Role, RoleResponse>();

            CreateMap<Publisher, PublisherResponse>();
            CreateMap<PublisherCreateRequest, Publisher>();
            CreateMap<PublisherUpdateRequest, Publisher>();

            CreateMap<Author, AuthorResponse>();
            CreateMap<AuthorCreateRequest, Author>();
            CreateMap<AuthorUpdateRequest, Author>();

            CreateMap<Book, BookResponse>();
            CreateMap<BookCreateRequest, Book>();
            CreateMap<BookUpdateRequest, Book>();
        }
    }
}
