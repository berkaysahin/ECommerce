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

public class CourseControllerTests
{
    private CourseController _sut;
    private readonly Mock<ICourseService> _courseService = new Mock<ICourseService>();
    
    public CourseControllerTests()
    {
        _sut = new CourseController(_courseService.Object);
    }
    
    #region GetAll
    
    [Test]
    public async Task GetAll_ShouldReturnCourses_WhenCoursesExist()
    {
        // Arrange
        CourseDTO course = new CourseDTO
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = Guid.NewGuid().ToString(),
            Picture = "null",
            CreatedTime = DateTime.UtcNow,
            CategoryId = Guid.NewGuid().ToString()
        };

        _courseService
            .Setup(f => f.GetAllAsync())
            .ReturnsAsync(Response<List<CourseDTO>>.Success(new List<CourseDTO>() { course }, 200));
        
        // Act
        var courseResult = (ObjectResult)(await _sut.GetAll());

        // Assert
        Assert.GreaterOrEqual(((Response<List<CourseDTO>>)(courseResult.Value)).Data.Count, 1);
    }
    
    [Test]
    public async Task GetAll_ShouldReturnCourseCountZero_WhenCourseDoesntExist()
    {
        // Arrange
        _courseService
            .Setup(f => f.GetAllAsync())
            .ReturnsAsync(Response<List<CourseDTO>>.Success(new List<CourseDTO>(), 200));

        // Act
        var courseResult = (ObjectResult)(await _sut.GetAll());

        // Assert
        Assert.Zero(((Response<List<CourseDTO>>)(courseResult.Value)).Data.Count);
    }
    
    #endregion

    #region GetById

    [Test]
    public async Task GetById_ShouldReturnCourse_WhenCourseExist()
    {
        // Arrange
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "is simply dummy"
        };
        
        CourseDTO course = new CourseDTO
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = Guid.NewGuid().ToString(),
            Picture = "null",
            CreatedTime = DateTime.UtcNow,
            CategoryId = category.Id
        };

        _courseService
            .Setup(f => f.GetByIdAsync(course.Id))
            .ReturnsAsync(Response<CourseDTO>.Success(course, 200));
        
        // Act
        var courseResult = (ObjectResult)(await _sut.GetById(course.Id));

        // Assert
        Assert.That(((Response<CourseDTO>)(courseResult.Value)).Data.Id, Is.EqualTo(course.Id));
    }

    [Test]
    public async Task GetById_ShouldReturnIsSuccessfulFalse_WhenCourseDoesntExist()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        
        _courseService
            .Setup(f => f.GetByIdAsync(userId))
            .ReturnsAsync(Response<CourseDTO>.Fail("Course not found", 404));
        // Act
        var courseResult = (ObjectResult)(await _sut.GetById(userId));

        // Assert
        Assert.False(((Response<CourseDTO>)(courseResult.Value)).IsSuccessful);
    }

    #endregion
    
    #region GetAllByUserId

    [Test]
    public async Task GetAllByUserId_ShouldReturnCoursesByUser_WhenCourseExist()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "is simply dummy"
        };
        
        CourseDTO course = new CourseDTO
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = userId,
            Picture = "null",
            CreatedTime = DateTime.UtcNow,
            CategoryId = category.Id
        };

        _courseService
            .Setup(f => f.GetAllByUserIdAsync(course.UserId))
            .ReturnsAsync(Response<List<CourseDTO>>.Success(new List<CourseDTO>() { course }, 200));

        // Act
        var courseResult = (ObjectResult)(await _sut.GetAllByUserId(userId));

        // Assert
        Assert.GreaterOrEqual(((Response<List<CourseDTO>>)(courseResult.Value)).Data.Count, 1);
    }

    [Test]
    public async Task GetAllByUserId_ShouldReturnCourseCountZero_WhenUserDontHaveCourse()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        
        _courseService
            .Setup(f => f.GetAllByUserIdAsync(userId))
            .ReturnsAsync(Response<List<CourseDTO>>.Success(new List<CourseDTO>(), 200));
        
        // Act
        var courseResult = (ObjectResult)(await _sut.GetAllByUserId(userId));

        // Assert
        Assert.Zero(((Response<List<CourseDTO>>)(courseResult.Value)).Data.Count);
    }

    #endregion

    #region Create

    [Test]
    public async Task Create_ShouldReturnCourse_WhenCourseCreated()
    {
        // Arrange
        CourseCreateDTO course = new CourseCreateDTO
        {
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = Guid.NewGuid().ToString(),
            Picture = "null",
            CategoryId = Guid.NewGuid().ToString(),
        };
        
        CourseDTO returnCourse = new CourseDTO
        {
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = course.UserId,
            Picture = "null",
            CategoryId = course.CategoryId,
        };

        _courseService
            .Setup(f => f.CreateAsync(course))
            .ReturnsAsync(Response<CourseDTO>.Success(returnCourse, 200));

        // Act
        var courseResult = (ObjectResult)(await _sut.Create(course));

        // Assert
        Assert.NotNull(((Response<CourseDTO>)(courseResult.Value)).Data);
    }

    [Test]
    public async Task Create_ShouldReturnIsSuccessfulFalse_WhenCourseNotCreated()
    {
        // Arrange
        _courseService
            .Setup(f => f.CreateAsync(null as CourseCreateDTO))
            .ReturnsAsync(Response<CourseDTO>.Fail("Course can not null", 404));
        
        // Act
        var courseResult = (ObjectResult)(await _sut.Create(null as CourseCreateDTO));

        // Assert
        Assert.False(((Response<CourseDTO>)(courseResult.Value)).IsSuccessful);
    }

    #endregion
    
    #region Update

    [Test]
    public async Task Update_ShouldReturnIsSuccessTrue_WhenCourseUpdated()
    {
        // Arrange
        CourseUpdateDTO course = new CourseUpdateDTO
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = Guid.NewGuid().ToString(),
            Picture = "null",
            CategoryId = Guid.NewGuid().ToString()
        };

        _courseService
            .Setup(f => f.UpdateAsync(course))
            .ReturnsAsync(Response<NoContent>.Success(204));

        // Act
        var courseResult = (ObjectResult)(await _sut.Update(course));

        // Assert
        Assert.True(((Response<NoContent>)(courseResult.Value)).IsSuccessful);
    }

    [Test]
    public async Task Update_ShouldReturnIsSuccessfulFalse_WhenCourseNotUpdated()
    {
        // Arrange
        _courseService
            .Setup(f => f.UpdateAsync(null as CourseUpdateDTO))
            .ReturnsAsync(Response<NoContent>.Fail("Course not found", 404));

        // Act
        var courseResult = (ObjectResult)(await _sut.Update(null as CourseUpdateDTO));

        // Assert
        Assert.False(((Response<NoContent>)(courseResult.Value)).IsSuccessful);
    }

    #endregion

    #region Delete

    [Test]
    public async Task Delete_ShouldReturnIsSuccessfulTrue_WhenCourseUpdated()
    {
        // Arrange
        Course course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = Guid.NewGuid().ToString(),
            Picture = "null",
            CreatedTime = DateTime.UtcNow,
            CategoryId = Guid.NewGuid().ToString()
        };
        
        _courseService
            .Setup(f => f.DeleteAsync(course.Id))
            .ReturnsAsync(Response<NoContent>.Success(204));
        
        // Act
        var courseResult = (ObjectResult)(await _sut.Delete(course.Id));

        // Assert
        Assert.True(((Response<NoContent>)(courseResult.Value)).IsSuccessful);
    }

    [Test]
    public async Task Delete_ShouldReturnIsSuccessFalse_WhenCourseNotUpdated()
    {
        // Arrange
        Course course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = Guid.NewGuid().ToString(),
            Picture = "null",
            CreatedTime = DateTime.UtcNow,
            CategoryId = Guid.NewGuid().ToString()
        };
        
        _courseService
            .Setup(f => f.DeleteAsync(course.Id))
            .ReturnsAsync(Response<NoContent>.Fail("Course not found", 404));

        // Act
        var courseResult = (ObjectResult)(await _sut.Delete(course.Id));

        // Assert
        Assert.False(((Response<NoContent>)(courseResult.Value)).IsSuccessful);
    }

    #endregion
}
