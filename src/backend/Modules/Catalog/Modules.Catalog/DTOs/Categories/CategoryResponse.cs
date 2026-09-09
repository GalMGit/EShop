namespace Modules.Catalog.DTOs.Categories;

public sealed record CategoryResponse(
    string Name, 
    Guid Id,
    Guid? ParentId);