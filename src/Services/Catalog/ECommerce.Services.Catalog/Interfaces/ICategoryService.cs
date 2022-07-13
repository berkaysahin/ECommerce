using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Models;
using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Catalog.Interfaces;

internal interface ICategoryService
{
    Task<Response<List<CategoryDTO>>> GetAllAsync();

    Task<Response<CategoryDTO>> CreateAsync(Category category);

    Task<Response<CategoryDTO>> GetByIdAsync(string id);
}
