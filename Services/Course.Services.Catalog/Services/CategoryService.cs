using AutoMapper;
using Course.Services.Catalog.DTOs;
using Course.Services.Catalog.Interfaces;
using Course.Services.Catalog.Models;
using Course.Services.Catalog.Settings;
using Course.Shared.DTOs;
using MongoDB.Driver;

namespace Course.Services.Catalog.Services;

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
