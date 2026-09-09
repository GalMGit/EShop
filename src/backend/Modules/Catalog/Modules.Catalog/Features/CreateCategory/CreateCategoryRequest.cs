namespace Modules.Catalog.Features.CreateCategory;

public sealed record CreateCategoryRequest(
    string Name, 
    Guid? ParentId);