namespace ECommerce.Services.Discount.Interfaces;

public interface IDiscountRepository
{
    Task<List<Models.Discount>> GetAll();
    Task<Models.Discount?> GetById(int id);
    Task<Models.Discount?> GetByCodeAndUserId(string code, string userId);
    Task<int> Save(Models.Discount discount);
    Task<int> Update(Models.Discount discount);
    Task<int> Delete(Models.Discount discount);
}
