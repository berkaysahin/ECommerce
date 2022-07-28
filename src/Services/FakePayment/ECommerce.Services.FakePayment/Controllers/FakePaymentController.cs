using ECommerce.Services.FakePayment.DTOs;
using ECommerce.Shared.ControllerBases;
using ECommerce.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.FakePayment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FakePaymentController : CustomBaseController
{
    [HttpPost]
    public IActionResult ReceivePayment(PaymentDTO paymentDto)
    {
        return CreateActionResultInstance(Response<NoContent>.Success(200));
    }
}
