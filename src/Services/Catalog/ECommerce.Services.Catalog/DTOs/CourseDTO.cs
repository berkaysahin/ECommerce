namespace ECommerce.Services.Catalog.DTOs;

internal class CourseDTO
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string UserId { get; set; }

    public string Picture { get; set; }

    public DateTime CreatedTime { get; set; }

    public string CategoryId { get; set; }

    public FeatureDTO Feature { get; set; }

    public CategoryDTO Category { get; set; }
}
