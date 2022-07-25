using ECommerce.Services.Order.Application.DTOs;
using ECommerce.Shared.DTOs;
using MediatR;

namespace ECommerce.Services.Order.Application.Queries;

public class GetOrdersByUserIdQuery : IRequest<Response<List<OrderDTO>>>
{
    public string UserId { get; set; }
}
