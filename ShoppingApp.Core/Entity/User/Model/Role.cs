using System.ComponentModel.DataAnnotations;
using ShoppingApp.Core.Common.Models;

namespace ShoppingApp.Core.Entity.User.Model;

public class Role
{
    [Key] 
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    public string Name { get; set; }
    
    public string? Description { get; set; }

    public virtual List<UserRole> RoleUsers { get; set; }
}