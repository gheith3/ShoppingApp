using System.ComponentModel.DataAnnotations;
using ShoppingApp.Core.Common.Models;

namespace ShoppingApp.Core.Entity.User.Dto;

public class RoleDto
{
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    public string? Description { get; set; }
}