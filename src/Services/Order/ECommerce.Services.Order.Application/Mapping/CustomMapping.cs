using AutoMapper;
using ECommerce.Services.Order.Application.DTOs;
using ECommerce.Services.Order.Domain.OrderAggregate;

namespace ECommerce.Services.Order.Application.Mapping;

public class CustomMapping : Profile
{
    public CustomMapping()
    {
        CreateMap<Domain.OrderAggregate.Order, OrderDTO>().ReverseMap();
        CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        CreateMap<Address, AddressDTO>().ReverseMap();
    }
}
