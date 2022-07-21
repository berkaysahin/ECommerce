using ECommerce.Services.Discount.Interfaces;
using ECommerce.Services.Discount.Services;
using Moq;

namespace ECommerce.Services.Discount.UnitTests.Services;

public class DiscountServiceTests
{
    private readonly DiscountService _sut;
    private readonly Mock<IDiscountRepository> _discountRespositoryMock = new Mock<IDiscountRepository>();

    public DiscountServiceTests()
    {
        _sut = new DiscountService(_discountRespositoryMock.Object);
    }
    
    #region GetAllAsync
    
    [Test]
    public async Task GetAllAsync_ShouldReturnDiscounts_WhenDiscountExist()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = "ABC"
        };

        _discountRespositoryMock
            .Setup(f => f.GetAll())
            .ReturnsAsync(new List<Models.Discount>() { discount });

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.GreaterOrEqual(result.Data.Count, 1);
    }
    
    [Test]
    public async Task GetAllAsync_ShouldReturnDiscountCountZero_WhenDiscountDoesntExist()
    {
        // Arrange
        _discountRespositoryMock
            .Setup(f => f.GetAll())
            .ReturnsAsync(new List<Models.Discount>());

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Zero(result.Data.Count);
    }

    #endregion

    #region GetByIdAsync

    [Test]
    public async Task GetByIdAsync_ShouldReturnDiscount_WhenDiscountExist()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = "ABC"
        };

        _discountRespositoryMock
            .Setup(f => f.GetById(discount.Id))
            .ReturnsAsync(discount);

        // Act
        var result = await _sut.GetByIdAsync(discount.Id);

        // Assert
        Assert.That(result.Data.Id, Is.EqualTo(discount.Id));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnIsSuccessfulFalse_WhenDiscountDoesntExist()
    {
        // Arrange
        _discountRespositoryMock
            .Setup(f => f.GetById(1))
            .ReturnsAsync(null as Models.Discount);

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        Assert.False(result.IsSuccessful);
    }

    #endregion
    
    #region GetByCodeAndUserIdAsync

    [Test]
    public async Task GetByCodeAndUserIdAsync_ShouldReturnDiscountByCodeAndUser_WhenDiscountExist()
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
        
        _discountRespositoryMock
            .Setup(f => f.GetByCodeAndUserId(discount.Code, discount.UserId))
            .ReturnsAsync(discount);

        // Act
        var result = await _sut.GetByCodeAndUserIdAsync(discount.Code, discount.UserId);

        // Assert
        Assert.NotNull(result.Data);
    }

    [Test]
    public async Task GetByCodeAndUserIdAsync_ShouldReturnDiscountNull_WhenUserDontHaveDiscount()
    {
        // Arrange
        Models.Discount discount = new Models.Discount
        {
            Id = 1,
            UserId = Guid.NewGuid().ToString(),
            Rate = 10,
            Code = Guid.NewGuid().ToString()
        };
        
        _discountRespositoryMock
            .Setup(f => f.GetByCodeAndUserId(discount.Code, discount.UserId))
            .ReturnsAsync(null as Models.Discount);

        // Act
        var courseResult = await _sut.GetByCodeAndUserIdAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        // Assert
        Assert.Null(courseResult.Data);
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
        
        _discountRespositoryMock
            .Setup(f => f.Save(It.IsAny<Models.Discount>()))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _sut.SaveAsync(discount);

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Test]
    public async Task SaveAsync_ShouldReturnIsSuccessfulFalse_WhenDiscountNotCreated()
    {
        // Arrange
        _discountRespositoryMock
            .Setup(f => f.Save(It.IsAny<Models.Discount>()))
            .Returns(Task.FromResult(0));

        // Act
        var result = await _sut.SaveAsync(null as Models.Discount);

        // Assert
        Assert.False(result.IsSuccessful);
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

        _discountRespositoryMock
            .Setup(f => f.Update(discount))
            .ReturnsAsync(1);
        
        _discountRespositoryMock
            .Setup(f => f.GetById(discount.Id))
            .ReturnsAsync(discount);

        // Act
        var result = await _sut.UpdateAsync(discount);

        // Assert
        Assert.True(result.IsSuccessful);
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
        
        _discountRespositoryMock
            .Setup(f => f.Update(new Models.Discount()))
            .ReturnsAsync(0);

        _discountRespositoryMock
            .Setup(f => f.GetById(discount.Id))
            .ReturnsAsync(null as Models.Discount);
        
        // Act
        var result = await _sut.UpdateAsync(discount);

        // Assert
        Assert.False(result.IsSuccessful);
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
        
        _discountRespositoryMock
            .Setup(f => f.Delete(discount))
            .ReturnsAsync(1);
        
        _discountRespositoryMock
            .Setup(f => f.GetById(discount.Id))
            .ReturnsAsync(discount);

        // Act
        var result = await _sut.DeleteAsync(discount.Id);

        // Assert
        Assert.True(result.IsSuccessful);
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
        
        _discountRespositoryMock
            .Setup(f => f.Delete(discount))
            .ReturnsAsync(0);

        // Act
        var result = await _sut.DeleteAsync(discount.Id);

        // Assert
        Assert.False(result.IsSuccessful);
    }

    #endregion
}
