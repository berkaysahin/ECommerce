using ECommerce.Services.Order.Infrastructure.Interfaces;
using ECommerce.Services.Order.Infrastructure.Repositories;
using ECommerce.Shared.Messages;
using MassTransit;

namespace ECommerce.Services.Order.Application.Consumers;

public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
{
    private readonly IOrderRepository _orderRepository;

    public CourseNameChangedEventConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
    {
        var orderItems = await _orderRepository.GetOrderItemsByProductId(item => item.ProductId == context.Message.CourseId);
        
        orderItems.ForEach(item =>
        {
            item.UpdateOrderItem(context.Message.UpdatedName, item.PictureURL, item.Price);
        });

        await _orderRepository.UpdateOrderItems(orderItems);
    }
}
