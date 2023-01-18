using ShoppingApp.Microservices.Users.Services;

namespace ShoppingApp.Microservices.Users.Startup;

public static class Services
{
    public static IServiceCollection AppServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        return services;
    }
}