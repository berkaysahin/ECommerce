using ECommerce.Services.Order.Domain.Core;

namespace ECommerce.Services.Order.Domain.OrderAggregate;

public class OrderItem : Entity
{
    public string ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string PictureURL { get; private set; }
    public Decimal Price { get; private set; }

    public OrderItem()
    {
        
    }
    
    public OrderItem(string productId, string productName, string pictureUrl, decimal price)
    {
        ProductId = productId;
        ProductName = productName;
        PictureURL = pictureUrl;
        Price = price;
    }

    public void UpdateOrderItem(string productName, string pictureUrl, decimal price)
    {
        ProductName = productName;
        PictureURL = pictureUrl;
        Price = price;
    }
}
