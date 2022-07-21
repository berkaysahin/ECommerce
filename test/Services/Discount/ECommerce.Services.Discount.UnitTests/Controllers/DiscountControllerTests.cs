using ECommerce.Services.Discount.Controllers;
using ECommerce.Services.Discount.Interfaces;
using ECommerce.Shared.DTOs;
using ECommerce.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerce.Services.Discount.UnitTests.Controllers;

public class DiscountControllerTests
{
    private DiscountController _sut;
    private readonly Mock<IDiscountService> _discountServiceMock = new Mock<IDiscountService>();
    private readonly Mock<ISharedIdentityService> _identityServiceMock = new Mock<ISharedIdentityService>();

    public DiscountControllerTests()
    {
        _sut = new DiscountController(_discountServiceMock.Object, _identityServiceMock.Object);
    }
    
    #region GetAll
    
    [Test]
    public async Task GetAll_ShouldReturnDiscounts_WhenDiscountExist()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = "ABC"
        };

        _discountServiceMock
            .Setup(f => f.GetAllAsync())
            .ReturnsAsync(Response<List<Models.Discount>>.Success(new List<Models.Discount>() { discount }, 200));

        // Act
        var result = (ObjectResult)(await _sut.GetAll());

        // Assert
        Assert.GreaterOrEqual(((Response<List<Models.Discount>>)(result.Value)).Data.Count, 1);
    }
    
    [Test]
    public async Task GetAll_ShouldReturnDiscountCountZero_WhenDiscountDoesntExist()
    {
        // Arrange
        _discountServiceMock
            .Setup(f => f.GetAllAsync())
            .ReturnsAsync(Response<List<Models.Discount>>.Success(new List<Models.Discount>(), 200));

        // Act
        var result = (ObjectResult)(await _sut.GetAll());

        // Assert
        Assert.Zero(((Response<List<Models.Discount>>)(result.Value)).Data.Count);
    }

    #endregion

    #region GetById

    [Test]
    public async Task GetById_ShouldReturnDiscount_WhenDiscountExist()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = "ABC"
        };

        _discountServiceMock
            .Setup(f => f.GetByIdAsync(discount.Id))
            .ReturnsAsync(Response<Models.Discount>.Success(discount, 200));

        // Act
        var result = (ObjectResult)(await _sut.GetById(discount.Id));

        // Assert
        Assert.That(((Response<Models.Discount>)(result.Value)).Data.Id, Is.EqualTo(discount.Id));
    }

    [Test]
    public async Task GetById_ShouldReturnIsSuccessfulFalse_WhenDiscountDoesntExist()
    {
        // Arrange
        _discountServiceMock
            .Setup(f => f.GetByIdAsync(1))
            .ReturnsAsync(Response<Models.Discount>.Fail("Discount not found", 404));

        // Act
        var result = (ObjectResult)(await _sut.GetById(1));

        // Assert
        Assert.False(((Response<Models.Discount>)(result.Value)).IsSuccessful);
    }

    #endregion
    
    #region GetByCode

    [Test]
    public async Task GetByCodeAndUserId_ShouldReturnDiscountByCodeAndUser_WhenDiscountExist()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = userId,
            Rate = 10,
            Code = Guid.NewGuid().ToString()
        };
        
        _discountServiceMock
            .Setup(f => f.GetByCodeAndUserIdAsync(discount.Code, discount.UserId))
            .ReturnsAsync(Response<Models.Discount>.Success(discount, 200));
        
        _identityServiceMock
            .Setup(f => f.GetUserId)
            .Returns(userId);

        // Act
        var result = (ObjectResult)(await _sut.GetByCode(discount.Code));

        // Assert
        Assert.NotNull(((Response<Models.Discount>)(result.Value)).Data);
    }

    [Test]
    public async Task GetByCodeAndUserId_ShouldReturnDiscountNull_WhenUserDontHaveDiscount()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = Guid.NewGuid().ToString()
        };
        
        _discountServiceMock
            .Setup(f => f.GetByCodeAndUserIdAsync(discount.Code, discount.UserId))
            .ReturnsAsync(Response<Models.Discount>.Fail("Discount not found", 404));
        
        _identityServiceMock
            .Setup(f => f.GetUserId)
            .Returns(discount.UserId);
        
        // Act
        var result = (ObjectResult)(await _sut.GetByCode(discount.Code));

        // Assert
        Assert.Null(((Response<Models.Discount>)(result.Value)).Data);
    }

    #endregion

    #region SaveAsync

    [Test]
    public async Task SaveAsync_ShouldReturnIsSuccessfulTrue_WhenDiscountCreated()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = Guid.NewGuid().ToString()
        };
        
        _discountServiceMock
            .Setup(f => f.SaveAsync(It.IsAny<Models.Discount>()))
            .ReturnsAsync(Response<NoContent>.Success(204));

        // Act
        var result = (ObjectResult)(await _sut.Save(discount));

        // Assert
        Assert.True(((Response<NoContent>)(result.Value)).IsSuccessful);
    }

    [Test]
    public async Task SaveAsync_ShouldReturnIsSuccessfulFalse_WhenDiscountNotCreated()
    {
        // Arrange
        _discountServiceMock
            .Setup(f => f.SaveAsync(It.IsAny<Models.Discount>()))
            .ReturnsAsync(Response<NoContent>.Fail("An error occurred while adding", 500));

        // Act
        var result = (ObjectResult)(await _sut.Save(null as Models.Discount));

        // Assert
        Assert.False(((Response<NoContent>)(result.Value)).IsSuccessful);
    }

    #endregion
    
    #region UpdateAsync

    [Test]
    public async Task UpdateAsync_ShouldReturnIsSuccessTrue_WhenDiscountUpdated()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = Guid.NewGuid().ToString()
        };

        _discountServiceMock
            .Setup(f => f.UpdateAsync(discount))
            .ReturnsAsync(Response<NoContent>.Success(204));

        // Act
        var result = (ObjectResult)(await _sut.Update(discount));

        // Assert
        Assert.True(((Response<NoContent>)(result.Value)).IsSuccessful);
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnIsSuccessfulFalse_WhenDiscountNotUpdated()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = Guid.NewGuid().ToString()
        };
        
        _discountServiceMock
            .Setup(f => f.UpdateAsync(discount))
            .ReturnsAsync(Response<NoContent>.Fail("An error occurred while updating", 500));
        
        // Act
        var result = (ObjectResult)(await _sut.Update(discount));

        // Assert
        Assert.False(((Response<NoContent>)(result.Value)).IsSuccessful);
    }

    #endregion

    #region DeleteAsync

    [Test]
    public async Task DeleteAsync_ShouldReturnIsSuccessfulTrue_WhenDiscountDeleted()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = Guid.NewGuid().ToString()
        };
        
        _discountServiceMock
            .Setup(f => f.DeleteAsync(discount.Id))
            .ReturnsAsync(Response<NoContent>.Success(204));

        // Act
        var result = (ObjectResult)(await _sut.Delete(discount.Id));

        // Assert
        Assert.True(((Response<NoContent>)(result.Value)).IsSuccessful);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnIsSuccessFalse_WhenDiscountNotDeleted()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = Guid.NewGuid().ToString()
        };
        
        _discountServiceMock
            .Setup(f => f.DeleteAsync(discount.Id))
            .ReturnsAsync(Response<NoContent>.Fail("An error occurred while updating", 500));

        // Act
        var result = (ObjectResult)(await _sut.Delete(discount.Id));

        // Assert
        Assert.False(((Response<NoContent>)(result.Value)).IsSuccessful);
    }

    #endregion
}
