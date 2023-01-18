using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Core.Common.Interfaces;
using ShoppingApp.Core.Common.Models;
using ShoppingApp.Core.Entity.Customer.Dto;
using ShoppingApp.Core.Entity.Customer.Model;
using ShoppingApp.Microservices.Customers.Data;

namespace ShoppingApp.Microservices.Customers.Services;

public class CustomersRepository : ICustomersRepository
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMapper _mapper;

    public CustomersRepository(AppDbContext context,
        IWebHostEnvironment hostingEnvironment, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
    }
    
    public IQueryable<Customer> GetQuery()
    {
        return _context.Customers
            .OrderByDescending(r => r.CreatedAt)
            .AsQueryable();
    }

    public IQueryable<Customer> GetFilterQuery(IQueryable<Customer> query, string? searchQuery = null, Dictionary<string, object>? filters = null)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(r =>
                    r.Name.Contains(searchQuery));
            }
            
            if (filters == null || !filters.Any())
            {
                return query;
            }
            
            if (filters.ContainsKey("PhoneNumber"))
            {
                query = query.Where(r => r.PhoneNumber == filters["PhoneNumber"].ToString());
            }

            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Query Was Get Error : {e.Message}");
        }
        return query;
    }

    public async Task<Customer> GetRecord(string id)
    {
        var record = await GetQuery()
            .FirstOrDefaultAsync(r => r.Id == id);
        
        if (record == null)
            throw new ApiException("record is not found",
                404,
                nameof(id));
        
        return record;
    }

    public async Task<ApiResponse<List<ListItem>>> List(string? searchQuery = null,
        Dictionary<string, object>? args = null)
    {
        var response = new ApiResponse<List<ListItem>>();
        try
        {
            var records = GetFilterQuery(
                GetQuery().Where(r => r.IsActive),
                searchQuery, args);
            
            response.Data = await records
                .Select(r => new ListItem
                {
                    Id = r.Id,
                    Value = $"{r.PhoneNumber} - {r.Name}"
                }).ToListAsync();
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


    public async Task<ApiResponse<CustomerDto>> Get(string id)
    {
        var response = new ApiResponse<CustomerDto>();
        try
        {
            var record = await GetRecord(id);
            response.Data = _mapper.Map<CustomerDto>(record);
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
                response.Errors.Add("server error", exception.Message);
            }
            response.StatusCode = 500;
            response.Errors.Add("_500", "server error");
        }

        return response;
    }
    public async Task CheckModifyRecord(Customer record, ModifyCustomerDto request)
    {
        if ((record.PhoneNumber != request.PhoneNumber)
            &&
            await _context.Customers.AnyAsync(r =>
                r.PhoneNumber == request.PhoneNumber))
        {
            throw new ApiException("this phone already registered",
                101,
                nameof(request.Name));
        }
    }

    public async Task<ApiResponse<ModifyCustomerDto>> Create(ModifyCustomerDto request)
    {
        var response = new ApiResponse<ModifyCustomerDto>();
        try
        {
            await CheckModifyRecord(new Customer(), request);
            var record = _mapper.Map<Customer>(request);
            record.Id = Guid.NewGuid().ToString("N");
            
            await _context.Customers.AddAsync(record);
            var res = await _context.SaveChangesAsync();
            if (res <= 0) 
                throw new ApiException("there is some issue when try to Customer record at database",
                    101);

            return await GetModifyRecord(record.Id);
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
    public async Task<ApiResponse<ModifyCustomerDto>> Update(ModifyCustomerDto request)
    {
        var response = new ApiResponse<ModifyCustomerDto>();
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                throw new ApiException("Id Is Required",
                    101,
                    nameof(request.Id));
            }

            var record = await GetRecord(request.Id);

            await CheckModifyRecord(record, request);

            record.UpdatedAt = DateTime.Now;
            record.Name = request.Name;
            record.Email = request.Email;
            record.PhoneNumber = request.PhoneNumber;
            record.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return await GetModifyRecord(record.Id);
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
    public async Task<ApiResponse<CustomerDto>> UpdateActivation(string id)
    {
        var response = new ApiResponse<CustomerDto>();
        try
        {
            var record = await GetRecord(id);
            record.IsActive = !record.IsActive;
            record.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<CustomerDto>(record);
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
                response.Errors.Add("server error", exception.Message);
            }
            response.StatusCode = 500;
            response.Errors.Add("_500", "server error");
        }

        return response;
    }
    public async Task<ApiResponse<ModifyCustomerDto>> GetModifyRecord(string id)
    {
        var response = new ApiResponse<ModifyCustomerDto>();
        try
        {
            var record = await GetRecord(id);
            response.Data = _mapper.Map<ModifyCustomerDto>(record);
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
    public async Task<ApiResponse<bool>> Delete(string id)
    {
        var response = new ApiResponse<bool>();
        try
        {
            var record = await GetRecord(id);
            record.IsActive = false;
            record.DeletedAt = DateTime.Now;
            record.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            response.Data = true;
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
                response.Errors.Add("server error", exception.Message);
            }
            response.StatusCode = 500;
            response.Errors.Add("_500", "server error");
        }

        return response;
    }
}