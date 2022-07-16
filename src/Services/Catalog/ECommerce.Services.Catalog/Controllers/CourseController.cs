using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : CustomBaseController
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _courseService.GetAllAsync();
        return CreateActionResultInstance(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _courseService.GetByIdAsync(id);
        return CreateActionResultInstance(response);
    }
    
    [HttpGet]
    [Route("/api/[controller]/GetAllByUserId/{userId}")]
    public async Task<IActionResult> GetAllByUserId(string userId)
    {
        var response = await _courseService.GetAllByUserIdAsync(userId);
        return CreateActionResultInstance(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDTO courseCreateDto)
    {
        var response = await _courseService.CreateAsync(courseCreateDto);
        return CreateActionResultInstance(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(CourseUpdateDTO courseUpdateDto)
    {
        var response = await _courseService.UpdateAsync(courseUpdateDto);
        return CreateActionResultInstance(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _courseService.DeleteAsync(id);
        return CreateActionResultInstance(response);
    }
}
