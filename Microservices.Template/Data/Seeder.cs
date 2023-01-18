using Microsoft.EntityFrameworkCore;

namespace Microservices.Template.Data;

public class Seeder
{
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        try
        {
            //init database, seed master data to database
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            //migrate database
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            //seed 
            
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }

}