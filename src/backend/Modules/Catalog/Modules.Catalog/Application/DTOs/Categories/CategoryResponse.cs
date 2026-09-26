namespace Modules.Catalog.Application.DTOs.Categories;

public sealed record CategoryResponse(
    string Name, 
    Guid Id);