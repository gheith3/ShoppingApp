using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Core.Common.Interfaces;
using ShoppingApp.Core.Common.Models;
using ShoppingApp.Core.Entity.User.Dto;
using ShoppingApp.Core.Entity.User.Model;
using ShoppingApp.Microservices.Users.Data;

namespace ShoppingApp.Microservices.Users.Services;

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMapper _mapper;

    public UsersRepository(AppDbContext context,
        IWebHostEnvironment hostingEnvironment, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
    }

    public IQueryable<User> GetQuery()
    {
        return _context.Users
            .OrderByDescending(r => r.CreatedAt)
            .Include(r => r.UsersRole)
            .ThenInclude(r => r.Role)
            .AsQueryable();
    }

    public IQueryable<User> GetFilterQuery(IQueryable<User> query, string? searchQuery = null,
        Dictionary<string, object>? filters = null)
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

    public async Task<User> GetRecord(string id)
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


    public async Task<ApiResponse<UserDto>> Get(string id)
    {
        var response = new ApiResponse<UserDto>();
        try
        {
            var record = await GetRecord(id);
            response.Data = _mapper.Map<UserDto>(record);
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

    public async Task CheckModifyRecord(User record, ModifyUserDto request)
    {
        if ((record.PhoneNumber != request.PhoneNumber)
            &&
            await _context.Users.AnyAsync(r =>
                r.PhoneNumber == request.PhoneNumber))
        {
            throw new ApiException("this phone already registered",
                101,
                nameof(request.Name));
        }
    }

    public async Task UpdateUserRoles(string id, List<string> userRoles)
    {
        _context.UsersRoles
            .RemoveRange(await _context.UsersRoles.Where(r => r.UserId == id).ToListAsync());
        await _context.SaveChangesAsync();

        var roles = await _context.Roles
            .Where(r => userRoles.Contains(r.Name))
            .ToListAsync();

        if (!roles.Any())
        {
            return;
        }

        await _context.UsersRoles
            .AddRangeAsync(
                roles.Select(r => new UserRole
                {
                    UserId = id,
                    RoleId = r.Id
                }));

        await _context.SaveChangesAsync();
    }

    public async Task<ApiResponse<ModifyUserDto>> Create(ModifyUserDto request)
    {
        var response = new ApiResponse<ModifyUserDto>();
        try
        {
            await CheckModifyRecord(new User(), request);
            var record = _mapper.Map<User>(request);
            record.Id = Guid.NewGuid().ToString("N");

            await _context.Users.AddAsync(record);
            var res = await _context.SaveChangesAsync();
            if (res <= 0)
                throw new ApiException("there is some issue when try to User record at database",
                    101);

            await UpdateUserRoles(record.Id, request.Roles);

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

    public async Task<ApiResponse<ModifyUserDto>> Update(ModifyUserDto request)
    {
        var response = new ApiResponse<ModifyUserDto>();
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
            
            await UpdateUserRoles(record.Id, request.Roles);

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


    public async Task<ApiResponse<UserDto>> UpdateActivation(string id)
    {
        var response = new ApiResponse<UserDto>();
        try
        {
            var record = await GetRecord(id);
            record.IsActive = !record.IsActive;
            record.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<UserDto>(record);
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

    public async Task<ApiResponse<ModifyUserDto>> GetModifyRecord(string id)
    {
        var response = new ApiResponse<ModifyUserDto>();
        try
        {
            var record = await GetRecord(id);
            response.Data = _mapper.Map<ModifyUserDto>(record);
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