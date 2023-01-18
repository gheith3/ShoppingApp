using Humanizer;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Core.Entity.User.Model;
using UserRole = ShoppingApp.Core.Entity.User.Enum.UserRole;

namespace ShoppingApp.Microservices.Users.Data;

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
            Roles(context);
            User(context);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }

    private static void Roles(AppDbContext context)
    {
        var oldRoles = context.Roles.Select(r => r.Name).ToList();
        var roles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>()
            .Select(r => new Role
            {
                Name = r.Humanize()
            })
            .ToList();

        context.Roles.AddRange(roles.Where(r => !oldRoles.Contains(r.Name)).ToList());
        context.SaveChanges();
    }

    private static void User(AppDbContext context)
    {
        if (context.UsersRoles.Any())
        {
            return;
        }

        var id = Guid.NewGuid().ToString("N");
        var roles = context.Roles
            .Select(r => new Core.Entity.User.Model.UserRole
            {
                UserId = id,
                RoleId = r.Id
            })
            .ToList();
        
        context.Users.Add(new User
        {
            Id = id,
            Name = "Root User",
            PhoneNumber = "93839229",
            Email = "admin@gmail.com",
            UsersRole = roles
        });
        context.SaveChanges();
    }
}