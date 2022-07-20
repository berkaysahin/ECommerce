using AutoMapper;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Models;
using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Catalog.Services;

public class CourseService : ICourseService
{
    private readonly IMongoDbClient<Course> _mongoDb;
    private readonly IMapper _mapper;
    private readonly ICategoryService _categoryService;

    public CourseService(
        IMapper mapper, 
        IMongoDbClient<Course> mongoDb,
        ICategoryService categoryService
        )
    {
        _mongoDb = mongoDb;
        _mongoDb.Setup("Courses");
        
        _mapper = mapper;

        _categoryService = categoryService;
    }
    
    public async Task<Response<List<CourseDTO>>> GetAllAsync()
    {
        var courses = await _mongoDb.FindAsync(course => true);

        if (courses is null || !courses.Any())
            courses = new List<Course>();
        
        foreach (var course in courses)
            course.Category = _mapper.Map<Category>((await _categoryService.GetByIdAsync(course.CategoryId)).Data);

        var data = _mapper.Map<List<CourseDTO>>(courses);
        return Response<List<CourseDTO>>.Success(data, 200);
    }
    
    public async Task<Response<CourseDTO>> GetByIdAsync(string id)
    {
        var course = await _mongoDb.FindByIdAsync(item => item.Id == id);

        if (course is null)
            return Response<CourseDTO>.Fail("Course not found", 404);

        course.Category = _mapper.Map<Category>((await _categoryService.GetByIdAsync(course.CategoryId)).Data);
        
        return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), 200);
    }
    
    public async Task<Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId)
    {
        var courses = await _mongoDb.FindAsync(item => item.UserId == userId);

        if (courses is null || !courses.Any())
            courses = new List<Course>();
        
        foreach (var course in courses)
            course.Category = _mapper.Map<Category>((await _categoryService.GetByIdAsync(course.CategoryId)).Data);

        var data = _mapper.Map<List<CourseDTO>>(courses);
        return Response<List<CourseDTO>>.Success(data, 200);
    }

    public async Task<Response<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDto)
    {
        if (courseCreateDto is null)
            return Response<CourseDTO>.Fail("Course can not null", 404);
            
        var newCourse = _mapper.Map<Course>(courseCreateDto);
        newCourse.CreatedTime = DateTime.UtcNow;
        
        await _mongoDb.InsertOneAsync(newCourse);
        
        var data = _mapper.Map<CourseDTO>(newCourse);
        return Response<CourseDTO>.Success(data, 200);
    }

    public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDto)
    {
        var updateCourse = _mapper.Map<Course>(courseUpdateDto);

        var result = await _mongoDb.FindOneAndReplace(item => item.Id == courseUpdateDto.Id, updateCourse);

        if (result is null)
            return Response<NoContent>.Fail("Course not found", 404);

        return Response<NoContent>.Success(204);
    }

    public async Task<Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _mongoDb.DeleteOneAsync(item => item.Id == id);
        
        if (result > 0)
            return Response<NoContent>.Success(204);

        return Response<NoContent>.Fail("Course not found", 404);
    }
}
