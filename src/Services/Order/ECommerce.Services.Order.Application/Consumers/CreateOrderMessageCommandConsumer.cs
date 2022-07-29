using ECommerce.Services.Order.Infrastructure.Interfaces;
using ECommerce.Shared.Messages;
using MassTransit;

namespace ECommerce.Services.Order.Application.Consumers;

public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderMessageCommandConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
    {
        var newAddress = new Domain.OrderAggregate.Address
        (
            context.Message.Address.Province, 
            context.Message.Address.District, 
            context.Message.Address.Street, 
            context.Message.Address.ZipCode, 
            context.Message.Address.Line
        );

        var order = new Domain.OrderAggregate.Order(context.Message.BuyerId, newAddress);
        
        context.Message.OrderItems.ForEach(item =>
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.PictureURL, item.Price);
        });

        await _orderRepository.CreateOrder(order);
    }
}
