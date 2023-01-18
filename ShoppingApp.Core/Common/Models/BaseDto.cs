using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Core.Common.Models;

public class BaseDto
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public string ActivationStatus => IsActive ? "Active" : "Disabled";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } 
}