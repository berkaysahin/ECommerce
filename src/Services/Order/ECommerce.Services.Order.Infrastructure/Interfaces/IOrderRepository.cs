using System.Linq.Expressions;

namespace ECommerce.Services.Order.Infrastructure.Interfaces;

public interface IOrderRepository
{
    Task<List<Domain.OrderAggregate.Order>> GetOrdersByUserId(Expression<Func<Domain.OrderAggregate.Order, bool>> expression);
    Task CreateOrder(Domain.OrderAggregate.Order order);
}
