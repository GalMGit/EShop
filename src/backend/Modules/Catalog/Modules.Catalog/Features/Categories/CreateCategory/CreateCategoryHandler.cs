using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Categories;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Mapping.Categories;

namespace Modules.Catalog.Features.Categories.CreateCategory;

public sealed class CreateCategoryHandler(
    CatalogDbContext context)
{
    public async Task<Result<CategoryResponse>> Handle(
        CreateCategoryCommand command, 
        CancellationToken ct)
    {
        var categoryExists = await context.Categories
            .AnyAsync(x => 
                x.Name == command.Request.Name, ct);

        if (categoryExists)
            return Result<CategoryResponse>.Failure(
                CategoryErrors.CategoryExists);

        if (command.Request.ParentId is not null)
        {
            var parentCategoryExists = await context.Categories
                .AnyAsync(x => 
                    x.ParentId == command.Request.ParentId, ct);
        
            if(!parentCategoryExists)
                return Result<CategoryResponse>.Failure(
                    CategoryErrors.ParentNotFound);
        }
        
        var category = new Category
        {
            Id = Guid.CreateVersion7(),
            Name = command.Request.Name,
            ParentId = command.Request.ParentId,
            IsActive = true
        };

        await context.Categories.AddAsync(
            category, ct);

        await context.SaveChangesAsync(ct);

        return Result<CategoryResponse>.Success(
            category.ToDto());
    }
}