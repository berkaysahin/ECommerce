using System.Linq.Expressions;
using AutoMapper;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Mapping;
using ECommerce.Services.Catalog.Models;
using ECommerce.Services.Catalog.Services;
using Moq;

namespace ECommerce.Services.Catalog.UnitTests.Services;

public class CourseServiceTests
{
    private readonly CourseService _sut;
    private readonly Mock<IMongoDbClient<Course>> _courseMongoDbClientMock = new Mock<IMongoDbClient<Course>>();
    private readonly Mock<IMongoDbClient<Category>> _categoryMongoDbClientMock = new Mock<IMongoDbClient<Category>>();
    private readonly IMapper _mapper;

    public CourseServiceTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        _mapper = mappingConfig.CreateMapper();

        _sut = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
    }

    #region GetAllAsync
    
    [Test]
    public async Task GetAllAsync_ShouldReturnCourses_WhenCoursesExist()
    {
        // Arrange
        Course course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CreatedTime = DateTime.Now,
            CategoryId = "79351164-be60-49c0-a83b-99a4aff14f53"
        };

        _courseMongoDbClientMock
            .Setup(f => f.FindAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(new List<Course>() { course });

        // Act
        var courseResult = await _sut.GetAllAsync();

        // Assert
        Assert.GreaterOrEqual(courseResult.Data.Count, 1);
    }
    
    [Test]
    public async Task GetAllAsync_ShouldReturnCourseCountZero_WhenCourseDoesntExist()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.FindAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(new List<Course>());

        // Act
        var courseResult = await _sut.GetAllAsync();

        // Assert
        Assert.Zero(courseResult.Data.Count);
    }
    
    [Test]
    public async Task GetAllAsync_ShouldReturnCategoryCountZero_WhenCategoryAndCourseAreNull()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.FindAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(null as List<Course>);
        
        _categoryMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(null as Category);

        // Act
        var courseResult = await _sut.GetAllAsync();

        // Assert
        Assert.Zero(courseResult.Data.Count);
    }
    
    #endregion

    #region GetByIdAsync

    [Test]
    public async Task GetByIdAsync_ShouldReturnCourse_WhenCourseExist()
    {
        // Arrange
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "is simply dummy"
        };
        
        Course course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CreatedTime = DateTime.Now,
            CategoryId = category.Id
        };

        _courseMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(course);

        _categoryMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(category);

        // Act
        var courseResult = await _sut.GetByIdAsync(course.Id);

        // Assert
        Assert.That(courseResult.Data.Id, Is.EqualTo(course.Id));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnIsSuccessfulFalse_WhenCourseDoesntExist()
    {
        // Arrange
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "is simply dummy"
        };
        
        _categoryMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(null as Category);

        // Act
        var courseResult = await _sut.GetByIdAsync(Guid.NewGuid().ToString());

        // Assert
        Assert.False(courseResult.IsSuccessful);
    }

    #endregion
    
    #region GetAllByUserIdAsync

    [Test]
    public async Task GetAllByUserIdAsync_ShouldReturnCoursesByUser_WhenCourseExist()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "is simply dummy"
        };
        
        Course course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = userId,
            Picture = "null",
            CreatedTime = DateTime.Now,
            CategoryId = category.Id
        };

        _courseMongoDbClientMock
            .Setup(f => f.FindAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(new List<Course>() { course });

        _categoryMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(category);

        // Act
        var courseResult = await _sut.GetAllByUserIdAsync(userId);

        // Assert
        Assert.GreaterOrEqual(courseResult.Data.Count, 1);
    }

    [Test]
    public async Task GetAllByUserIdAsync_ShouldReturnCourseCountZero_WhenUserDontHaveCourse()
    {
        // Arrange
        Category category = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "is simply dummy"
        };
        
        _courseMongoDbClientMock
            .Setup(f => f.FindAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(null as List<Course>);
        
        _categoryMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(null as Category);

        // Act
        var courseResult = await _sut.GetAllByUserIdAsync(Guid.NewGuid().ToString());

        // Assert
        Assert.Zero(courseResult.Data.Count);
    }

    #endregion

    #region CreateAsync

    [Test]
    public async Task CreateAsync_ShouldReturnCourse_WhenCourseCreated()
    {
        // Arrange
        CourseCreateDTO course = new CourseCreateDTO
        {
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CategoryId = Guid.NewGuid().ToString(),
        };

        _courseMongoDbClientMock
            .Setup(f => f.InsertOneAsync(It.IsAny<Course>()))
            .Returns(Task.FromResult(typeof(void)));

        // Act
        var courseResult = await _sut.CreateAsync(course);

        // Assert
        Assert.NotNull(courseResult.Data);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnIsSuccessfulFalse_WhenCourseNotCreated()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.InsertOneAsync(It.IsAny<Course>()))
            .Returns(Task.FromResult(typeof(void)));

        // Act
        var courseResult = await _sut.CreateAsync(null as CourseCreateDTO);

        // Assert
        Assert.False(courseResult.IsSuccessful);
    }

    #endregion
    
    #region UpdateAsync

    [Test]
    public async Task UpdateAsync_ShouldReturnIsSuccessTrue_WhenCourseUpdated()
    {
        // Arrange
        Course course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CreatedTime = DateTime.Now,
            CategoryId = Guid.NewGuid().ToString()
        };

        _courseMongoDbClientMock
            .Setup(f => f.FindOneAndReplace(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Course>()))
            .ReturnsAsync(course);

        // Act
        var courseResult = await _sut.UpdateAsync(_mapper.Map<CourseUpdateDTO>(course));

        // Assert
        Assert.True(courseResult.IsSuccessful);
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnIsSuccessfulFalse_WhenCourseNotUpdated()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.FindOneAndReplace(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Course>()))
            .ReturnsAsync(null as Course);

        // Act
        var courseResult = await _sut.UpdateAsync(null as CourseUpdateDTO);

        // Assert
        Assert.False(courseResult.IsSuccessful);
    }

    #endregion

    #region DeleteAsync

    [Test]
    public async Task DeleteAsync_ShouldReturnIsSuccessfulTrue_WhenCourseUpdated()
    {
        // Arrange
        Course course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CreatedTime = DateTime.Now,
            CategoryId = Guid.NewGuid().ToString()
        };
        
        _courseMongoDbClientMock
            .Setup(f => f.DeleteOneAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(1);

        // Act
        var courseResult = await _sut.DeleteAsync(course.Id);

        // Assert
        Assert.True(courseResult.IsSuccessful);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnIsSuccessFalse_WhenCourseNotUpdated()
    {
        // Arrange
        Course course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Lorem",
            Description = "ipsum",
            Price = 0,
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CreatedTime = DateTime.Now,
            CategoryId = Guid.NewGuid().ToString()
        };
        
        _courseMongoDbClientMock
            .Setup(f => f.DeleteOneAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(0);

        // Act
        var courseResult = await _sut.DeleteAsync(course.Id);

        // Assert
        Assert.False(courseResult.IsSuccessful);
    }

    #endregion
}
