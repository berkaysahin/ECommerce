using ECommerce.Services.FakePayment.DTOs;
using ECommerce.Shared.ControllerBases;
using ECommerce.Shared.DTOs;
using ECommerce.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.FakePayment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FakePaymentController : CustomBaseController
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public FakePaymentController(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    [HttpPost]
    public async Task<IActionResult> ReceivePayment(PaymentDTO paymentDto)
    {
        // payment transactions take place..
        //----------------------------------
        
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

        var address = new Address
        {
            Province = paymentDto.Order.Address.Province,
            District = paymentDto.Order.Address.District,
            Street = paymentDto.Order.Address.Street,
            Line = paymentDto.Order.Address.Line
        };
        
        var createOrderMessageCommand = new CreateOrderMessageCommand
        {
            BuyerId = paymentDto.Order.BuyerId,
            Address = address
        };
        
        paymentDto.Order.OrderItems.ForEach(item =>
        {
            createOrderMessageCommand.OrderItems.Add(new OrderItem
            {
                PictureURL = item.PictureUrl,
                Price = item.Price,
                ProductId = item.ProductId,
                ProductName = item.ProductName
            });
        });

        await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
        
        return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Success(200));
    }
}
