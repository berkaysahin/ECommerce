using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Discount.Interfaces;

public interface IDiscountService
{
    Task<Response<List<Models.Discount>>> GetAllAsync();
    Task<Response<Models.Discount>> GetByIdAsync(int id);
    Task<Response<Models.Discount>> GetByCodeAndUserIdAsync(string code, string userId);
    Task<Response<NoContent>> SaveAsync(Models.Discount discount);
    Task<Response<NoContent>> UpdateAsync(Models.Discount discount);
    Task<Response<NoContent>> DeleteAsync(int id);
}
