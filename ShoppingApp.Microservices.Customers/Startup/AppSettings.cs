using ShoppingApp.Microservices.Customers.Data;

namespace ShoppingApp.Microservices.Customers.Startup;

public static class AppSettings
{
    public static WebApplication AppConfig(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection()
            .UseAuthentication()
            .UseAuthorization();
        
        app.MapControllers();
        
        return app;
    }


    public static WebApplication SeedDatabase(this WebApplication app)
    {
        try
        {
            Console.WriteLine("Start seed Database");
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            Seeder.InitializeDatabase(services);
            Console.WriteLine("Finish seed Database");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Database Error : {e.Message}");
        }
        return app;
    } 
}