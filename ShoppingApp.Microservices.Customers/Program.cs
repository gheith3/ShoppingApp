using ShoppingApp.Core.JwtAuth;
using ShoppingApp.Microservices.Customers.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .DatabaseConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AppAuthentication()
    .AppServices()
    .AppControllers()
    .Http()
    .Swagger()
    .Mapper();

var app = builder.Build();
app.SeedDatabase()
    .AppConfig()
    .UseCors();

app.Run();