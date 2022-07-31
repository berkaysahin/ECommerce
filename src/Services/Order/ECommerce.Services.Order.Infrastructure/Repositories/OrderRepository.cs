using System.Linq.Expressions;
using ECommerce.Services.Order.Domain.OrderAggregate;
using ECommerce.Services.Order.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Services.Order.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;
    
    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Domain.OrderAggregate.Order>> GetOrdersByUserId(Expression<Func<Domain.OrderAggregate.Order, bool>> expression)
    {
        return await _context.Orders.Include(item => item.OrderItems)
            .Where(expression).ToListAsync();
    }

    public async Task CreateOrder(Domain.OrderAggregate.Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Domain.OrderAggregate.OrderItem>> GetOrderItemsByProductId(Expression<Func<Domain.OrderAggregate.OrderItem, bool>> expression)
    {
        return await _context.OrderItem
            .Where(expression).ToListAsync();
    }
    
    public async Task<int> UpdateOrderItems(List<OrderItem> orderItems)
    {
        _context.ChangeTracker.Clear();
        
        orderItems.ForEach(item =>
        {
            _context.Update(item);
        });
        
        return await _context.SaveChangesAsync();
    }
}
