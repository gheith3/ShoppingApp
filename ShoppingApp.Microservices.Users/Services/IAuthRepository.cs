using ShoppingApp.Core.Common.Models;
using ShoppingApp.Core.Entity.User.Dto;

namespace ShoppingApp.Microservices.Users.Services;

public interface IAuthRepository
{
    Task<ApiResponse<LoginResponseDto>> LoginWithPassword(LoginWithPasswordDto request);
}