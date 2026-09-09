namespace Modules.Catalog.Features.Categories.CreateCategory;

public sealed record CreateCategoryRequest(
    string Name, 
    Guid? ParentId);