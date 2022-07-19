using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Services.Basket.DTOs;
using ECommerce.Services.Basket.Interfaces;
using ECommerce.Shared.ControllerBases;
using ECommerce.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.Basket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketController : CustomBaseController
{
    private readonly IBasketService _basketService;
    private readonly ISharedIdentityService _identityService;

    public BasketController(IBasketService basketService, ISharedIdentityService identityService)
    {
        _basketService = basketService;
        _identityService = identityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasket()
    {
        return CreateActionResultInstance(await _basketService.GetBasket(_identityService.GetUserId));
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveOrUpdateBasket(BasketDTO basketDto)
    {
        var response = await _basketService.SaveOrUpdate(basketDto);
        return CreateActionResultInstance(response);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteBasket()
    {
        return CreateActionResultInstance(await _basketService.Delete(_identityService.GetUserId));
    }
}
