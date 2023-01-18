using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShoppingApp.Core.Common.Models;

public class ApiResponse<T>
{
    [AllowNull] public T? Data { get; set; } = default;

    [Required] public bool IsSuccess => Data != null;

    [Required] public int StatusCode { get; set; } = 200;
    
    public Dictionary<string, string> Errors { get; set; } = new();
}