using AutoMapper;
using ShoppingApp.Core.Entity.Customer.Dto;
using ShoppingApp.Core.Entity.Customer.Model;

namespace ShoppingApp.Microservices.Customers.Mappers;

public class CustomerMapper : Profile
{
    public CustomerMapper()
    {
        MapCustomer();
    }

    private void MapCustomer()
    {
        CreateMap<Customer, CustomerDto>()
            .ReverseMap();
        
        CreateMap<Customer, ModifyCustomerDto>()
            .ReverseMap();
    }
}