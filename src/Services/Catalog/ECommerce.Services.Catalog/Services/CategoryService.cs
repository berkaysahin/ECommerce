using AutoMapper;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Models;
using ECommerce.Services.Catalog.Settings;
using ECommerce.Shared.DTOs;
using MongoDB.Driver;

namespace ECommerce.Services.Catalog.Services;

public class CategoryService : ICategoryService
{
    private readonly IMongoDbClient<Category> _mongoDb;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, IMongoDbClient<Category> mongoDbClient)
    {
        _mongoDb = mongoDbClient;
        _mongoDb.Setup("Categories");

        _mapper = mapper;
    }

    public async Task<Response<List<CategoryDTO>>> GetAllAsync()
    {
        var categories = await _mongoDb.FindAsync(category => true);
        var data = _mapper.Map<List<CategoryDTO>>(categories);
        return Response<List<CategoryDTO>>.Success(data, 200);
    }

    public async Task<Response<CategoryDTO>> CreateAsync(Category category)
    {
        await _mongoDb.InsertOneAsync(category);
        return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
    }

    public async Task<Response<CategoryDTO>> GetByIdAsync(string id)
    {
        var category = (await _mongoDb.FindAsync(category => category.Id == id)).FirstOrDefault();

        if (category is null)
            return Response<CategoryDTO>.Fail("Category not found", 404);

        return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
    }
}
