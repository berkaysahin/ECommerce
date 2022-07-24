using ECommerce.Services.Order.Domain.OrderAggregate;

namespace ECommerce.Services.Order.Application.DTOs;

public class OrderDTO
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public AddressDTO Address { get; set; }
    public string BuyerId { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
}
