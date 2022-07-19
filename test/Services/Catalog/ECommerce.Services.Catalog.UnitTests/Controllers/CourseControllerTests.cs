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

public class CourseControllerTests
{
    private CourseController _sut;
    private readonly Mock<IMongoDbClient<Course>> _courseMongoDbClientMock = new Mock<IMongoDbClient<Course>>();
    private readonly Mock<IMongoDbClient<Category>> _categoryMongoDbClientMock = new Mock<IMongoDbClient<Category>>();
    private readonly IMapper _mapper;
    private ICourseService _courseService;
    
    public CourseControllerTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        _mapper = mappingConfig.CreateMapper();

        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);
    }
    
    #region GetAll
    
    [Test]
    public async Task GetAll_ShouldReturnCourses_WhenCoursesExist()
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
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.GetAll());

        // Assert
        Assert.GreaterOrEqual(((Response<List<CourseDTO>>)(courseResult.Value)).Data.Count, 1);
    }
    
    [Test]
    public async Task GetAll_ShouldReturnCourseCountZero_WhenCourseDoesntExist()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.FindAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(new List<Course>());
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.GetAll());

        // Assert
        Assert.Zero(((Response<List<CourseDTO>>)(courseResult.Value)).Data.Count);
    }
    
    [Test]
    public async Task GetAll_ShouldReturnCategoryCountZero_WhenCategoryAndCourseAreNull()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.FindAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(null as List<Course>);
        
        _categoryMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(null as Category);
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

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
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.GetById(course.Id));

        // Assert
        Assert.That(((Response<CourseDTO>)(courseResult.Value)).Data.Id, Is.EqualTo(course.Id));
    }

    [Test]
    public async Task GetById_ShouldReturnIsSuccessfulFalse_WhenCourseDoesntExist()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(null as Course);
        
        _categoryMongoDbClientMock
            .Setup(f => f.FindByIdAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(null as Category);
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.GetById(Guid.NewGuid().ToString()));

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
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.GetAllByUserId(userId));

        // Assert
        Assert.GreaterOrEqual(((Response<List<CourseDTO>>)(courseResult.Value)).Data.Count, 1);
    }

    [Test]
    public async Task GetAllByUserId_ShouldReturnCourseCountZero_WhenUserDontHaveCourse()
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
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.GetAllByUserId(Guid.NewGuid().ToString()));

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
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CategoryId = Guid.NewGuid().ToString(),
        };

        _courseMongoDbClientMock
            .Setup(f => f.InsertOneAsync(It.IsAny<Course>()))
            .Returns(Task.FromResult(typeof(void)));
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.Create(course));

        // Assert
        Assert.NotNull(((Response<CourseDTO>)(courseResult.Value)).Data);
    }

    [Test]
    public async Task Create_ShouldReturnIsSuccessfulFalse_WhenCourseNotCreated()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.InsertOneAsync(It.IsAny<Course>()))
            .Returns(Task.FromResult(typeof(void)));
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

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
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.Update(_mapper.Map<CourseUpdateDTO>(course)));

        // Assert
        Assert.True(((Response<NoContent>)(courseResult.Value)).IsSuccessful);
    }

    [Test]
    public async Task Update_ShouldReturnIsSuccessfulFalse_WhenCourseNotUpdated()
    {
        // Arrange
        _courseMongoDbClientMock
            .Setup(f => f.FindOneAndReplace(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Course>()))
            .ReturnsAsync(null as Course);
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

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
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CreatedTime = DateTime.Now,
            CategoryId = Guid.NewGuid().ToString()
        };
        
        _courseMongoDbClientMock
            .Setup(f => f.DeleteOneAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(1);
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

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
            UserId = "9d85174d-e6bf-4b51-888a-ae569b978080",
            Picture = "null",
            CreatedTime = DateTime.Now,
            CategoryId = Guid.NewGuid().ToString()
        };
        
        _courseMongoDbClientMock
            .Setup(f => f.DeleteOneAsync(It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(0);
        
        _courseService = new CourseService(_mapper, _courseMongoDbClientMock.Object, new CategoryService(_mapper, _categoryMongoDbClientMock.Object));
        _sut = new CourseController(_courseService);

        // Act
        var courseResult = (ObjectResult)(await _sut.Delete(course.Id));

        // Assert
        Assert.False(((Response<NoContent>)(courseResult.Value)).IsSuccessful);
    }

    #endregion
}
