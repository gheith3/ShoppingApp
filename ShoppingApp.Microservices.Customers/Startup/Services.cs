using ShoppingApp.Microservices.Customers.Services;

namespace ShoppingApp.Microservices.Customers.Startup;

public static class Services
{
    public static IServiceCollection AppServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomersRepository, CustomersRepository>();
        return services;
    }
}