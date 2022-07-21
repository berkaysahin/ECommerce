using ECommerce.Services.Discount.Interfaces;
using ECommerce.Shared.ControllerBases;
using ECommerce.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.Discount.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountController : CustomBaseController
{
    private readonly IDiscountService _discountService;
    private readonly ISharedIdentityService _identityService;

    public DiscountController(IDiscountService discountService, ISharedIdentityService identityService)
    {
        _discountService = discountService;
        _identityService = identityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _discountService.GetAllAsync();
        return CreateActionResultInstance(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _discountService.GetByIdAsync(id);
        return CreateActionResultInstance(result);
    }

    [HttpGet]
    [Route("/api/[controller]/[action]/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var userId = _identityService.GetUserId;
        var result = await _discountService.GetByCodeAndUserIdAsync(code, userId);
        return CreateActionResultInstance(result);
    }

    [HttpPost]
    public async Task<IActionResult> Save(Models.Discount discount)
    {
        var result = await _discountService.SaveAsync(discount);
        return CreateActionResultInstance(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(Models.Discount discount)
    {
        var result = await _discountService.UpdateAsync(discount);
        return CreateActionResultInstance(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _discountService.DeleteAsync(id);
        return CreateActionResultInstance(result);
    }
}
