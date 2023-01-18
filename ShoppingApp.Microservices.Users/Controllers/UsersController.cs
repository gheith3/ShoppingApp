

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Core.Common.Models;
using ShoppingApp.Core.Entity.User.Dto;
using ShoppingApp.Microservices.Users.Services;

namespace ShoppingApp.Microservices.Users.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,Root")]
public class UsersController : ControllerBase
{
    private readonly IUsersRepository _repository;

    public UsersController(IUsersRepository repository)
    {
        _repository = repository;
    }
    
    [HttpPost("users-list")]
    public async Task<ActionResult<ApiResponse<List<ListItem>>>> List(string? searchQuery = null, Dictionary<string, object>? args = null)
    {
        return await _repository.List(searchQuery, args);
    }
    
    
    [HttpPost("create-user")]
    public async Task<ActionResult<ApiResponse<ModifyUserDto>>> Create(ModifyUserDto request)
    {
        return await _repository.Create(request);
    }

    [HttpGet("get-user/{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Get(string id)
    {
        return await _repository.Get(id);
    }

    [HttpGet("get-user-to-modify/{id}")]
    public async Task<ActionResult<ApiResponse<ModifyUserDto>>> GetTModify(string id)
    {
        return await _repository.GetModifyRecord(id);
    }

    [HttpPut("update-user")]
    public async Task<ActionResult<ApiResponse<ModifyUserDto>>> Update(ModifyUserDto request)
    {
        return await _repository.Update(request);
    }

    [HttpPut("update-user-activation/{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateActivation(string id)
    {
        return await _repository.UpdateActivation(id);
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(string id)
    {
        return await _repository.Delete(id);
    }
}