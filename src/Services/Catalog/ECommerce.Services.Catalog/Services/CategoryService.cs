using AutoMapper;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Models;
using ECommerce.Services.Catalog.Settings;
using ECommerce.Shared.DTOs;
using MongoDB.Driver;

namespace ECommerce.Services.Catalog.Services;

internal class CategoryService : ICategoryService
{
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<List<CategoryDTO>>> GetAllAsync()
    {
        var categories = await _categoryCollection.Find(category => true).ToListAsync();
        return Response<List<CategoryDTO>>.Success(_mapper.Map<List<CategoryDTO>>(categories), 200);
    }

    public async Task<Response<CategoryDTO>> CreateAsync(Category category)
    {
        await _categoryCollection.InsertOneAsync(category);
        return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
    }

    public async Task<Response<CategoryDTO>> GetByIdAsync(string id)
    {
        var category = await _categoryCollection.Find<Category>(category => category.Id == id).FirstOrDefaultAsync();

        if (category is null)
            return Response<CategoryDTO>.Fail("Category not found", 404);

        return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
    }
}
