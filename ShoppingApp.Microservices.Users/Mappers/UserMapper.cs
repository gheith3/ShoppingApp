using AutoMapper;
using ShoppingApp.Core.Entity.User.Dto;
using ShoppingApp.Core.Entity.User.Model;

namespace ShoppingApp.Microservices.Users.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        MapUser();
    }

    private void MapUser()
    {
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.Roles,
                opt
                    => opt.MapFrom(r => r.UsersRole.Select(i => i.Role.Name)))
            .ReverseMap();

        CreateMap<User, ModifyUserDto>()
            .ReverseMap();
    }
}