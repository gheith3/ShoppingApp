using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ShoppingApp.Core.Common.Models;

namespace ShoppingApp.Core.Entity.User.Dto;

public class UserDto : BaseDto
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}