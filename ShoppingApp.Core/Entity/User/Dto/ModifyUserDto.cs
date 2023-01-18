using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ShoppingApp.Core.Common.Models;

namespace ShoppingApp.Core.Entity.User.Dto;

public class ModifyUserDto
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = true;
    public List<string> Roles { get; set; }
}