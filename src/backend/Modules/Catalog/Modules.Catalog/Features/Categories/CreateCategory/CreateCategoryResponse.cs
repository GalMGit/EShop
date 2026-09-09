namespace Modules.Catalog.Features.Categories.CreateCategory;

public sealed record CreateCategoryResponse(
    string Name, 
    Guid Id,
    Guid? ParentId);
