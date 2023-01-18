using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoppingApp.Core.Common.Models;
using ShoppingApp.Core.Entity.User.Dto;
using ShoppingApp.Core.Entity.User.Model;
using ShoppingApp.Microservices.Users.Data;

namespace ShoppingApp.Microservices.Users.Services;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMapper _mapper;

    public AuthRepository(AppDbContext context,
        IWebHostEnvironment hostingEnvironment, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginWithPassword(LoginWithPasswordDto request)
    {
        var response = new ApiResponse<LoginResponseDto>();
        try
        {
            var user = await _context.Users
                .Include(r => r.UsersRole)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(r =>
                    r.PhoneNumber == request.Identifier ||
                    r.Email == request.Identifier);

            if (user == null) throw new ApiException("this user dose not exist", 101);

            if (!user.IsActive) throw new ApiException("this user account is disabled", 101);

            
            //todo: check password
            // if (result != PasswordVerificationResult.Success)
            //     throw new ApiException("user info is not correct", 101);

            var login = await Login(user);

            if (login == null) throw new ApiException("there is some issue when user try login", 101);

            response.Data = login;
        }
        catch (ApiException exception)
        {
            response.StatusCode = exception.ErrorCode;
            response.Errors.Add(exception.ErrorTitle, exception.Message);
        }
        catch (Exception exception)
        {
            if (!_hostingEnvironment.IsProduction())
            {
                response.StatusCode = 500;
                response.Errors.Add("server error", exception.Message);
            }

            //Log.Error("Error, Message {ExceptionMessage}", exception.Message);
        }

        return response;
    }


    private async Task<LoginResponseDto> Login(User user)
    {
        try
        {
            var userDto = _mapper.Map<UserDto>(user);
            var authClaims = new List<Claim>
            {
                new("Id", user.Id),
                new(ClaimTypes.Sid, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.Jti, user.Id),
                new(ClaimTypes.GivenName, user.Name),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            if (user.UsersRole.Any())
            {
                foreach (var role in user.UsersRole.Select(r => r.Role.Name))
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var authSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ShoppingApp.Core.JwtAuth.JwtConstants.Secret));
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(ShoppingApp.Core.JwtAuth.JwtConstants.LifeHours)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new LoginResponseDto
            {
                User = _mapper.Map<UserDto>(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiredAt = token.ValidTo
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }
}