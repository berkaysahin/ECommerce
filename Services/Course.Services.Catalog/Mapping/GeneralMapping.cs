using AutoMapper;
using Course.Services.Catalog.DTOs;
using Course.Services.Catalog.Models;

namespace Course.Services.Catalog.Mapping;

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
