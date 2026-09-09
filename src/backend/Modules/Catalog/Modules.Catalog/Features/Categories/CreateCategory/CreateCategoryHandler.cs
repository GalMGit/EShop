using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;

namespace Modules.Catalog.Features.Categories.CreateCategory;

public sealed class CreateCategoryHandler(
    CatalogDbContext context)
{
    public async Task<Result<CreateCategoryResponse>> Handle(
        CreateCategoryCommand command, 
        CancellationToken ct)
    {
        var categoryExists = await context.Categories
            .AnyAsync(x => 
                x.Name == command.Request.Name, ct);

        if (categoryExists)
            return Result<CreateCategoryResponse>.Failure(
                CategoryErrors.CategoryExists);

        if (command.Request.ParentId is not null)
        {
            var parentCategoryExists = await context.Categories
                .AnyAsync(x => 
                    x.ParentId == command.Request.ParentId, ct);
        
            if(!parentCategoryExists)
                return Result<CreateCategoryResponse>.Failure(
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

        return Result<CreateCategoryResponse>.Success(
            new CreateCategoryResponse(
                category.Name, 
                category.Id, 
                category.ParentId));
    }
}