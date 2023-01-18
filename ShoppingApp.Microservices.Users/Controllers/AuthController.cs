

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Core.Common.Models;
using ShoppingApp.Core.Entity.User.Dto;
using ShoppingApp.Microservices.Users.Services;

namespace ShoppingApp.Microservices.Users.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly IAuthRepository _repository;

    public AuthController(IAuthRepository repository)
    {
        _repository = repository;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginWithPasswordDto request)
    {
        return await _repository.LoginWithPassword(request);
    }
    
}