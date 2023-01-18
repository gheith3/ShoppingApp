using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ShoppingApp.Core.Common.Models;

namespace ShoppingApp.Core.Entity.Customer.Model;

public class Customer : BaseModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    
    [AllowNull]
    public string Email { get; set; }
}