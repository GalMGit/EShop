namespace Modules.Catalog.Features.CreateCategory;

public sealed record CreateCategoryResponse(
    string Name, 
    Guid Id,
    Guid? ParentId);
