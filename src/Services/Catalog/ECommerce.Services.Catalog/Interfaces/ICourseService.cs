using ECommerce.Services.Catalog.DTOs;
using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Catalog.Interfaces;

public interface ICourseService
{
    Task<Response<List<CourseDTO>>> GetAllAsync();
    Task<Response<CourseDTO>> GetByIdAsync(string id);
    Task<Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId);
    Task<Response<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDto);
    Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDto);
    Task<Response<NoContent>> DeleteAsync(string id);
}
