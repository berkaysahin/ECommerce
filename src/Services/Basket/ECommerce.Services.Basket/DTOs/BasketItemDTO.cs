namespace ECommerce.Services.Basket.DTOs;

public class BasketItemDTO
{
    public int Quantity { get; set; }
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public decimal Price { get; set; }
}
