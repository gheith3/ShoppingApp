

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Core.Common.Models;
using ShoppingApp.Core.Entity.Customer.Dto;
using ShoppingApp.Microservices.Customers.Services;

namespace ShoppingApp.Microservices.Customers.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class CustomersController : ControllerBase
{
    private readonly ICustomersRepository _repository;

    public CustomersController(ICustomersRepository repository)
    {
        _repository = repository;
    }
    
    [HttpPost("customers-list")]
    public async Task<ActionResult<ApiResponse<List<ListItem>>>> List(string? searchQuery = null, Dictionary<string, object>? args = null)
    {
        return await _repository.List(searchQuery, args);
    }
    
    
    [HttpPost("create-customer")]
    public async Task<ActionResult<ApiResponse<ModifyCustomerDto>>> Create(ModifyCustomerDto request)
    {
        return await _repository.Create(request);
    }

    [HttpGet("get-customer/{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Get(string id)
    {
        return await _repository.Get(id);
    }

    [HttpGet("get-customer-to-modify/{id}")]
    public async Task<ActionResult<ApiResponse<ModifyCustomerDto>>> GetTModify(string id)
    {
        return await _repository.GetModifyRecord(id);
    }

    [HttpPut("update-customer")]
    public async Task<ActionResult<ApiResponse<ModifyCustomerDto>>> Update(ModifyCustomerDto request)
    {
        return await _repository.Update(request);
    }

    [HttpPut("update-customer-activation/{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateActivation(string id)
    {
        return await _repository.UpdateActivation(id);
    }

    [HttpDelete("delete-customer/{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(string id)
    {
        return await _repository.Delete(id);
    }
}