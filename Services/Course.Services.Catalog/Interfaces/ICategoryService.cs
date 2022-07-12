using Course.Services.Catalog.DTOs;
using Course.Services.Catalog.Models;
using Course.Shared.DTOs;

namespace Course.Services.Catalog.Interfaces;

internal interface ICategoryService
{
    Task<Response<List<CategoryDTO>>> GetAllAsync();

    Task<Response<CategoryDTO>> CreateAsync(Category category);

    Task<Response<CategoryDTO>> GetByIdAsync(string id);
}
