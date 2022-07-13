using System;
using System.Linq.Expressions;
using AutoMapper;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Mapping;
using ECommerce.Services.Catalog.Models;
using ECommerce.Services.Catalog.Services;
using ECommerce.Services.Catalog.Settings;
using MongoDB.Driver;
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
        Category categoryDTO = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem"
        };

        _mongoDbClientMock.Setup(f => f.FindAsync(It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(new List<Category>() { categoryDTO });

        // Act
        var category = await _sut.GetAllAsync();

        // Assert
        Assert.GreaterOrEqual(category.Data.Count, 1);
    }
}
