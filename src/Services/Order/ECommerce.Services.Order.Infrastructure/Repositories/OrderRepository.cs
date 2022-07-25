using System.Linq.Expressions;
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
}
