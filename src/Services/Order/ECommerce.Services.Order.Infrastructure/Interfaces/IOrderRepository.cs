using System.Linq.Expressions;
using ECommerce.Services.Order.Domain.OrderAggregate;

namespace ECommerce.Services.Order.Infrastructure.Interfaces;

public interface IOrderRepository
{
    Task<List<Domain.OrderAggregate.Order>> GetOrdersByUserId(Expression<Func<Domain.OrderAggregate.Order, bool>> expression);
    Task CreateOrder(Domain.OrderAggregate.Order order);

    Task<List<Domain.OrderAggregate.OrderItem>> GetOrderItemsByProductId(
        Expression<Func<Domain.OrderAggregate.OrderItem, bool>> expression);

    Task<int> UpdateOrderItems(List<OrderItem> orderItems);
}
