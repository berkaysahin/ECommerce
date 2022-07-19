using System.Linq.Expressions;
using AutoMapper;
using ECommerce.Services.Catalog.Controllers;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Mapping;
using ECommerce.Services.Catalog.Models;
using ECommerce.Services.Catalog.Services;
using ECommerce.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerce.Services.Catalog.UnitTests.Controllers;

public class CategoryControllerTests
{
    private CategoryController _sut;
    private readonly Mock<IMongoDbClient<Category>> _mongoDbClientMock = new Mock<IMongoDbClient<Category>>();
    private ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoryControllerTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        _mapper = mappingConfig.CreateMapper();
        
        _categoryService = new CategoryService(_mapper, _mongoDbClientMock.Object);
    }
    
    [Test]
    public async Task GetAll_ShouldReturnCategoryCount_WhenCategoriesExist()
    {
        // Arrange
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem"
        };

        _mongoDbClientMock
            .Setup(f => f.FindAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(new List<Category>() { category });
        
        _categoryService = new CategoryService(_mapper, _mongoDbClientMock.Object);
        _sut = new CategoryController(_categoryService);

        // Act
        var categoryResult = (ObjectResult)(await _sut.GetAll());

        // Assert
        Assert.GreaterOrEqual(((Response<List<CategoryDTO>>)(categoryResult.Value)).Data.Count, 1);
    }

    #region GetByIdAsync

    [Test]
    public async Task GetById_ShouldReturnCategory_WhenCategoryExist()
    {
        // Arrange
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem"
        };

        _mongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(category);
        
        _categoryService = new CategoryService(_mapper, _mongoDbClientMock.Object);
        _sut = new CategoryController(_categoryService);

        // Act
        var categoryResult = (ObjectResult)(await _sut.GetById(category.Id));

        // Assert
        Assert.That(((Response<CategoryDTO>)(categoryResult.Value)).Data.Id, Is.EqualTo(category.Id));
    }

    [Test]
    public async Task GetById_ShouldReturnFail_WhenCategoryDoesntExist()
    {
        // Arrange
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem"
        };

        _mongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(null as Category);
        
        _categoryService = new CategoryService(_mapper, _mongoDbClientMock.Object);
        _sut = new CategoryController(_categoryService);

        // Act
        var categoryResult = (ObjectResult)(await _sut.GetById(category.Id));

        // Assert
        Assert.That(categoryResult.StatusCode, Is.EqualTo(404));
    }

    #endregion

    #region CreateAsync

    [Test]
    public async Task Create_ShouldReturnCategory_WhenCategoryCreated()
    {
        // Arrange
        CategoryDTO category = new CategoryDTO()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem"
        };

        _mongoDbClientMock
            .Setup(f => f.InsertOneAsync(It.IsAny<Category>()))
            .Returns(Task.FromResult(typeof(void)));
        
        _categoryService = new CategoryService(_mapper, _mongoDbClientMock.Object);
        _sut = new CategoryController(_categoryService);

        // Act
        var categoryResult = (ObjectResult)(await _sut.Create(category));

        // Assert
        Assert.NotNull(((Response<CategoryDTO>)(categoryResult.Value)).Data);
    }

    [Test]
    public async Task Create_ShouldReturnFail_WhenCategoryNotCreated()
    {
        // Arrange

        _mongoDbClientMock
            .Setup(f => f.InsertOneAsync(It.IsAny<Category>()))
            .Returns(Task.FromResult(typeof(void)));
        
        _categoryService = new CategoryService(_mapper, _mongoDbClientMock.Object);
        _sut = new CategoryController(_categoryService);

        // Act
        var categoryResult = (ObjectResult)(await _sut.Create(null as CategoryDTO));

        // Assert
        Assert.Null(((Response<CategoryDTO>)(categoryResult.Value)).Data);
    }

    #endregion
}
