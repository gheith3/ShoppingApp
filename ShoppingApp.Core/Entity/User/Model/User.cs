using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ShoppingApp.Core.Common.Models;

namespace ShoppingApp.Core.Entity.User.Model;

public class User : BaseModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    
    [AllowNull]
    public string Email { get; set; }
    
    public virtual List<UserRole> UsersRole { get; set; }
}