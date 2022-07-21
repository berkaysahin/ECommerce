using System.Linq.Expressions;
using ECommerce.Services.Catalog.Controllers;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Models;
using ECommerce.Services.Catalog.Services;
using ECommerce.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerce.Services.Catalog.UnitTests.Controllers;

public class CategoryControllerTests
{
    private CategoryController _sut;
    private readonly Mock<ICategoryService> _discountServiceMock = new Mock<ICategoryService>();

    public CategoryControllerTests()
    {
        _sut = new CategoryController(_discountServiceMock.Object);
    }
    
    [Test]
    public async Task GetAll_ShouldReturnCategoryCount_WhenCategoriesExist()
    {
        // Arrange
        CategoryDTO category = new CategoryDTO()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem"
        };

        _discountServiceMock
            .Setup(f => f.GetAllAsync())
            .ReturnsAsync(Response<List<CategoryDTO>>.Success(new List<CategoryDTO>() { category }, 200));
        
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
        CategoryDTO category = new CategoryDTO()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem"
        };

        _discountServiceMock
            .Setup(f => f.GetByIdAsync(category.Id))
            .ReturnsAsync(Response<CategoryDTO>.Success(category, 200));

        // Act
        var categoryResult = (ObjectResult)(await _sut.GetById(category.Id));

        // Assert
        Assert.That(((Response<CategoryDTO>)(categoryResult.Value)).Data.Id, Is.EqualTo(category.Id));
    }

    [Test]
    public async Task GetById_ShouldReturnFail_WhenCategoryDoesntExist()
    {
        // Arrange
        CategoryDTO category = new CategoryDTO()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem"
        };

        _discountServiceMock
            .Setup(f => f.GetByIdAsync(category.Id))
            .ReturnsAsync(Response<CategoryDTO>.Fail("Category not found", 404));

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

        _discountServiceMock
            .Setup(f => f.CreateAsync(category))
            .ReturnsAsync(Response<CategoryDTO>.Success(category, 200));

        // Act
        var categoryResult = (ObjectResult)(await _sut.Create(category));

        // Assert
        Assert.NotNull(((Response<CategoryDTO>)(categoryResult.Value)).Data);
    }

    [Test]
    public async Task Create_ShouldReturnFail_WhenCategoryNotCreated()
    {
        // Arrange

        _discountServiceMock
            .Setup(f => f.CreateAsync(null as CategoryDTO))
            .ReturnsAsync(Response<CategoryDTO>.Fail("Category can not null", 400));

        // Act
        var categoryResult = (ObjectResult)(await _sut.Create(null as CategoryDTO));

        // Assert
        Assert.Null(((Response<CategoryDTO>)(categoryResult.Value)).Data);
    }

    #endregion
}
