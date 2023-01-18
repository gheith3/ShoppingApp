using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ShoppingApp.Core.Common.Models;

namespace ShoppingApp.Core.Entity.User.Model;

public class UserRole
{
    [Key] 
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public User User { get; set; }
    public string UserId { get; set; }
    
    public Role Role { get; set; }
    public string RoleId { get; set; }
}