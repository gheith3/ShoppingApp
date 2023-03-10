using Microsoft.EntityFrameworkCore;
using ShoppingApp.Core.Entity.Customer.Model;

namespace Microservices.Template.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    
    public DbSet<Customer> Customers { get; set; }
}