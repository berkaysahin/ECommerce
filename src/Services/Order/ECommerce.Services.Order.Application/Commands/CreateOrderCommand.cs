using ECommerce.Services.Order.Application.DTOs;
using ECommerce.Shared.DTOs;
using MediatR;

namespace ECommerce.Services.Order.Application.Commands;

public class CreateOrderCommand : IRequest<Response<CreatedOrderDTO>>
{
    public string BuyerId { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
    public AddressDTO Address { get; set; }
}
