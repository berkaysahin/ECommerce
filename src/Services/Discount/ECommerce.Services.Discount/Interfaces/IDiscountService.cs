using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Discount.Interfaces;

public interface IDiscountService
{
    Task<Response<List<Models.Discount>>> GetAll();
    Task<Response<Models.Discount>> GetById(int id);
    Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId);
    Task<Response<NoContent>> Save(Models.Discount discount);
    Task<Response<NoContent>> Update(Models.Discount discount);
    Task<Response<NoContent>> Delete(int id);
}
