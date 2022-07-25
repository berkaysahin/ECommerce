using ECommerce.Services.Order.Application.DTOs;
using ECommerce.Services.Order.Application.Mapping;
using ECommerce.Services.Order.Application.Queries;
using ECommerce.Services.Order.Infrastructure.Interfaces;
using ECommerce.Shared.DTOs;
using MediatR;

namespace ECommerce.Services.Order.Application.Handlers;

public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDTO>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByUserIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Response<List<OrderDTO>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByUserId(item => item.BuyerId == request.UserId);

        if (orders is null || !orders.Any())
            return Response<List<OrderDTO>>.Success(new List<OrderDTO>(), 200);

        var ordersDto = ObjectMapper.Mapper.Map<List<OrderDTO>>(orders);
        return Response<List<OrderDTO>>.Success(ordersDto, 200);
    }
}
