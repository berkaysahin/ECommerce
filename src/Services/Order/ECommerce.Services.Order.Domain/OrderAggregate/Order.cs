using ECommerce.Services.Order.Domain.Core;

namespace ECommerce.Services.Order.Domain.OrderAggregate;

/*
 * EF Core features
 * Owned Types
 * Shadow Property
 * Backing Field
 */

public class Order : Entity, IAggregateRoot
{
    public DateTime CreatedDate { get; private set; }
    public Address Address { get; private set; }
    public string BuyerId { get; private set; }

    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order(string buyerId, Address address)
    {
        _orderItems = new List<OrderItem>();
        CreatedDate = DateTime.UtcNow;
        BuyerId = buyerId;
        Address = address;
    }

    public void AddOrderItem(string productId, string productName, string pictureUrl, decimal price)
    {
        var existProduct = _orderItems.Any(item => item.ProductId == productId);

        if (existProduct)
            return;
        
        var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
        _orderItems.Add(newOrderItem);
    }

    public decimal GetTotalPrice => _orderItems.Sum(item => item.Price);
}
