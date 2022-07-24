namespace ECommerce.Services.Order.Application.DTOs;

public class OrderItemDTO
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string PictureURL { get; set; }
    public Decimal Price { get; set; }
}