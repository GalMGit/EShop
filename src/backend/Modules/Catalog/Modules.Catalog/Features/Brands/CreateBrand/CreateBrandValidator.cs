using FluentValidation;

namespace Modules.Catalog.Features.Brands.CreateBrand;

public sealed class CreateBrandValidator : AbstractValidator<CreateBrandRequest>
{
    public CreateBrandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MaximumLength(30).WithMessage("Название не может быть больше 30 символов");
    }
}