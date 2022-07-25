using ECommerce.Services.Order.Application.Commands;
using ECommerce.Services.Order.Application.Queries;
using ECommerce.Shared.ControllerBases;
using ECommerce.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _identityService;

        public OrderController(IMediator mediator, ISharedIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = _identityService.GetUserId });
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand request)
        {
            var response = await _mediator.Send(request);
            return CreateActionResultInstance(response);
        }
    }
}
