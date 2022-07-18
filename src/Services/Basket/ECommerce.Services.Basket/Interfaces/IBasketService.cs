using ECommerce.Services.Basket.DTOs;
using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Basket.Interfaces;

public interface IBasketService
{
    Task<Response<BasketDTO>> GetBasket(string userId);
    Task<Response<bool>> SaveOrUpdate(BasketDTO basketDto);
    Task<Response<bool>> Delete(string userId);
}
