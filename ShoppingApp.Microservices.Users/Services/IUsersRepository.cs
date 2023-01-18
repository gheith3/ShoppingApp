using ShoppingApp.Core.Common.Interfaces;
using ShoppingApp.Core.Entity.User.Dto;
using ShoppingApp.Core.Entity.User.Model;

namespace ShoppingApp.Microservices.Users.Services;

public interface IUsersRepository : ICrudRepository<User, UserDto, ModifyUserDto>
{
    
}