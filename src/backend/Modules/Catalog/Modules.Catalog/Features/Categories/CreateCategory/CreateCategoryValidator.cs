using FluentValidation;

namespace Modules.Catalog.Features.Categories.CreateCategory;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MaximumLength(100).WithMessage("Название не может быть больше 100 символов");
    }
}