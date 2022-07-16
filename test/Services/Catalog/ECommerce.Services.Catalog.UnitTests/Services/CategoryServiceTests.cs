using System.Linq.Expressions;
using AutoMapper;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Mapping;
using ECommerce.Services.Catalog.Models;
using ECommerce.Services.Catalog.Services;
using Moq;

namespace ECommerce.Services.Catalog.UnitTests.Services;

public class CategoryServiceTests
{
    private readonly CategoryService _sut;
    private readonly Mock<IMongoDbClient<Category>> _mongoDbClientMock = new Mock<IMongoDbClient<Category>>();

    public CategoryServiceTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        _sut = new CategoryService(mapper, _mongoDbClientMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnCategoryCount_WhenCategoriesExist()
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

        // Act
        var categoryResult = await _sut.GetAllAsync();

        // Assert
        Assert.GreaterOrEqual(categoryResult.Data.Count, 1);
    }

    #region GetByIdAsync

    [Test]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExist()
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

        // Act
        var categoryResult = await _sut.GetByIdAsync(category.Id);

        // Assert
        Assert.That(categoryResult.Data.Id, Is.EqualTo(category.Id));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnFail_WhenCategoryDoesntExist()
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

        // Act
        var categoryResult = await _sut.GetByIdAsync(category.Id);

        // Assert
        Assert.That(categoryResult.StatusCode, Is.EqualTo(404));
    }

    #endregion

    #region CreateAsync

    [Test]
    public async Task CreateAsync_ShouldReturnCategory_WhenCategoryCreated()
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

        // Act
        var categoryResult = await _sut.CreateAsync(category);

        // Assert
        Assert.NotNull(categoryResult.Data);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnFail_WhenCategoryNotCreated()
    {
        // Arrange

        _mongoDbClientMock
            .Setup(f => f.InsertOneAsync(It.IsAny<Category>()))
            .Returns(Task.FromResult(typeof(void)));

        // Act
        var categoryResult = await _sut.CreateAsync(null as CategoryDTO);

        // Assert
        Assert.Null(categoryResult.Data);
    }

    #endregion
}
