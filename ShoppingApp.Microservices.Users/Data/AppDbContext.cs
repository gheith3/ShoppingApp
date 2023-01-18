using Microsoft.EntityFrameworkCore;
using ShoppingApp.Core.Entity.Customer.Model;
using ShoppingApp.Core.Entity.User.Model;

namespace ShoppingApp.Microservices.Users.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UsersRoles { get; set; }
}