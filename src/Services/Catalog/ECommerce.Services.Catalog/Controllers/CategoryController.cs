using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : CustomBaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return CreateActionResultInstance(categories);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return CreateActionResultInstance(category);
    }

    public async Task<IActionResult> Create(CategoryDTO categoryDto)
    {
        var response = await _categoryService.CreateAsync(categoryDto);
        return CreateActionResultInstance(response);
    }  
}
