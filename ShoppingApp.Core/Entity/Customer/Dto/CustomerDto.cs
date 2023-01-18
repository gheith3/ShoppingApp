using ShoppingApp.Core.Common.Models;

namespace ShoppingApp.Core.Entity.Customer.Dto;

public class CustomerDto : BaseDto
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}