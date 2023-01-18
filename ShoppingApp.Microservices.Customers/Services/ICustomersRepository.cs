using ShoppingApp.Core.Common.Interfaces;
using ShoppingApp.Core.Entity.Customer.Dto;
using ShoppingApp.Core.Entity.Customer.Model;

namespace ShoppingApp.Microservices.Customers.Services;

public interface ICustomersRepository : ICrudRepository<Customer, CustomerDto, ModifyCustomerDto>
{
    
}