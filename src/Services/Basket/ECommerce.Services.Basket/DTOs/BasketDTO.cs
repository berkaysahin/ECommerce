namespace ECommerce.Services.Basket.DTOs;

public class BasketDTO
{
    public string UserId { get; set; }
    public string DiscountCode { get; set; }
    public List<BasketItemDTO> basketItems { get; set; }

    public decimal TotalPrice => basketItems.Sum(item => item.Price * item.Quantity);
}
