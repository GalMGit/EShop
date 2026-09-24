using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Mapping.Categories;
using Modules.Catalog.DTOs.Categories;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;

namespace Modules.Catalog.Features.Categories.GetCategory;

public sealed class GetCategoryHandler(
    CatalogDbContext context)
{
    public async Task<Result<CategoryResponse>> Handle(
        GetCategoryQuery query, 
        CancellationToken ct)
    {
        var category = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.Id == query.Id, ct);

        if (category is null)
            return Result<CategoryResponse>.Failure(
                CategoryErrors.NotFound);

        return Result<CategoryResponse>.Success(
            category.ToDto());
    }
}