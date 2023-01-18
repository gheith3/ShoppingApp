using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ShoppingApp.Microservices.Users.Data;

namespace ShoppingApp.Microservices.Users.Startup;

public static class ServiceSettings
{
    public static IServiceCollection DatabaseConnection(this IServiceCollection services,
        string? connectionStrings)
    {
        if (string.IsNullOrEmpty(connectionStrings))
        {
            throw new ArgumentNullException(nameof(connectionStrings), "Database Connection Strings Key is required, add it to appSettings then check program.cs");
        }
        
        try
        {
            Console.WriteLine("Start Connection To DB");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionStrings);
                Console.WriteLine("Connected");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return services;
    }
    
    public static IServiceCollection Cors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins(
                            "https://localhost:7196"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        return services;
    }


    public static IServiceCollection Http(this IServiceCollection services)
    {
        //add ability to access httpContext from any where by inject
        services.AddHttpContextAccessor();
        services.AddHttpClient();

        return services;
    }

    public static IServiceCollection Mapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }

    public static IServiceCollection Swagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IServiceCollection AppControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                options.SerializerSettings.DateParseHandling = DateParseHandling.None;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
                    {DateTimeStyles = DateTimeStyles.AssumeUniversal});
            });
        return services;
    }
}