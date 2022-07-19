using System.Text.Json;
using ECommerce.Services.Basket.DTOs;
using ECommerce.Services.Basket.Interfaces;
using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Basket.Services;

public class BasketService : IBasketService
{
    private readonly RedisService _redisService;

    public BasketService(RedisService redisService)
    {
        _redisService = redisService;
    }
    
    public async Task<Response<BasketDTO>> GetBasket(string userId)
    {
        var existBasket = await _redisService.GetDb().StringGetAsync(userId);

        if (String.IsNullOrEmpty(existBasket))
            return Response<BasketDTO>.Fail("Basket not found", 404);

        return Response<BasketDTO>.Success(JsonSerializer.Deserialize<BasketDTO>(existBasket), 200);
    }

    public async Task<Response<bool>> SaveOrUpdate(BasketDTO basketDto)
    {
        var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));
        
        return status ? 
            Response<bool>.Success(204) : 
            Response<bool>.Fail("Basket could not save or update", 500);
    }

    public async Task<Response<bool>> Delete(string userId)
    {
        var status = await _redisService.GetDb().KeyDeleteAsync(userId);
        
        return status ? 
            Response<bool>.Success(204) : 
            Response<bool>.Fail("Basket not found", 404);
    }
}
