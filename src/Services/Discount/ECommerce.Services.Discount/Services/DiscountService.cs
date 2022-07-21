using ECommerce.Services.Discount.Interfaces;
using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Discount.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<Response<List<Models.Discount>>> GetAllAsync()
    {
        var discounts = await _discountRepository.GetAll();
        return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
    }

    public async Task<Response<Models.Discount>> GetByIdAsync(int id)
    {
        var discount = await _discountRepository.GetById(id);

        if (discount is null)
            return Response<Models.Discount>.Fail("Discount not found", 404);

        return Response<Models.Discount>.Success(discount, 200);
    }

    public async Task<Response<Models.Discount>> GetByCodeAndUserIdAsync(string code, string userId)
    {
        var discount = await _discountRepository.GetByCodeAndUserId(code, userId);
        
        if (discount is null)
            return Response<Models.Discount>.Fail("Discount not found", 404);

        return Response<Models.Discount>.Success(discount, 200);
    }
    
    public async Task<Response<NoContent>> SaveAsync(Models.Discount discount)
    {
        var status = await _discountRepository.Save(discount); 

        if (status > 0)
            return Response<NoContent>.Success(204);
        
        return Response<NoContent>.Fail("An error occurred while adding", 500);
    }

    public async Task<Response<NoContent>> UpdateAsync(Models.Discount discount)
    {
        var discountData = await _discountRepository.GetById(discount.Id);
        
        if (discountData is null)
            return Response<NoContent>.Fail("Discount not found", 404);
        
        var status = await _discountRepository.Update(discount);
        
        if (status > 0)
            return Response<NoContent>.Success(204);
        
        return Response<NoContent>.Fail("An error occurred while updating", 500);
    }

    public async Task<Response<NoContent>> DeleteAsync(int id)
    {
        var discountData = await _discountRepository.GetById(id);
        
        if (discountData is null)
            return Response<NoContent>.Fail("Discount not found", 404);
        
        var status = await _discountRepository.Delete(discountData);
        
        if (status > 0)
            return Response<NoContent>.Success(204);
        
        return Response<NoContent>.Fail("An error occurred while updating", 500);
    }
}
