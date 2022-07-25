using ECommerce.Services.Order.Application.Commands;
using ECommerce.Services.Order.Application.DTOs;
using ECommerce.Services.Order.Domain.OrderAggregate;
using ECommerce.Services.Order.Infrastructure.Interfaces;
using ECommerce.Shared.DTOs;
using MediatR;

namespace ECommerce.Services.Order.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDTO>>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Response<CreatedOrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newAddress = new Address(request.AddressDto.Province, request.AddressDto.District, request.AddressDto.Street, request.AddressDto.ZipCode, request.AddressDto.Line);

        var newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);
        
        request.OrderItems.ForEach(item =>
        {
            newOrder.AddOrderItem(item.ProductId, item.ProductName, item.PictureURL, item.Price);
        });

        await _orderRepository.CreateOrder(newOrder);

        return Response<CreatedOrderDTO>.Success(new CreatedOrderDTO { OrderId = newOrder.Id }, 200);
    }
}
