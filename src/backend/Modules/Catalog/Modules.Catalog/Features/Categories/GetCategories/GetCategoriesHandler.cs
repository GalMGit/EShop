using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Mapping.Categories;
using Modules.Catalog.DTOs.Categories;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;

namespace Modules.Catalog.Features.Categories.GetCategories;

public sealed class GetCategoriesHandler(
    CatalogDbContext context)
{
    public async Task<Result<List<CategoryResponse>>> Handle(
        GetCategoriesQuery query,
        CancellationToken ct)
    {
        var categories = await context.Categories
            .AsNoTracking()
            .ToListAsync(ct);

        return Result<List<CategoryResponse>>.Success(
            categories
                .Select(x => x
                    .ToDto())
                .ToList());
    }
}