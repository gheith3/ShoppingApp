using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ShoppingApp.Core.Common;

namespace ShoppingApp.Core.Entity.Customer.Dto;

public class ModifyCustomerDto 
{
    public string? Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [AllowNull]
    public string Email { get; set; }
    public bool IsActive { get; set; } = true;
}