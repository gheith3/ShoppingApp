using Microservices.Template.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .DatabaseConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
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