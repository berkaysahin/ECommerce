using System.Diagnostics.CodeAnalysis;
using ECommerce.Services.Discount.Data;
using ECommerce.Services.Discount.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Services.Discount.Repositories;

[ExcludeFromCodeCoverage]
public class DiscountRepository : IDiscountRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DiscountRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Models.Discount>> GetAll()
    {
        return await _dbContext.Discounts.ToListAsync();
    }

    public async Task<Models.Discount?> GetById(int id)
    {
        return await _dbContext.Discounts.FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<Models.Discount?> GetByCodeAndUserId(string code, string userId)
    {
        return await _dbContext.Discounts.SingleOrDefaultAsync(item => item.UserId == userId && item.Code == code);
    }

    public async Task<int> Save(Models.Discount discount)
    {
        await _dbContext.Discounts.AddAsync(discount);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> Update(Models.Discount discount)
    {
        _dbContext.ChangeTracker.Clear();
        _dbContext.Update(discount);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> Delete(Models.Discount discount)
    {
        _dbContext.Entry(discount).State = EntityState.Deleted;
        _dbContext.Discounts.Remove(discount);
        
        return await _dbContext.SaveChangesAsync();
    }
}
