﻿using AutoMapper;
using ECommerce.Services.Catalog.DTOs;
using ECommerce.Services.Catalog.Models;

namespace ECommerce.Services.Catalog.Mapping;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        CreateMap<Models.Course, CourseDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Feature, FeatureDTO>().ReverseMap();

        CreateMap<Models.Course, CourseCreateDTO>().ReverseMap();
        CreateMap<Models.Course, CourseUpdateDTO>().ReverseMap();
    }
}