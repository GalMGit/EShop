using FluentValidation;

namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MaximumLength(200).WithMessage("Название не может быть больше 200 символов");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание не может быть пустым")
            .MaximumLength(5000).WithMessage("Название не может быть больше 5000 символов");
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Цена должна быть больше 0");

        RuleFor(x => x.Specifications)
            .NotEmpty().WithMessage("Спецификация не может быть пустой");
        
        RuleFor(x => x.BrandId)
            .NotEmpty()
            .WithMessage("BrandId не может быть пустым")
            .NotEqual(Guid.Empty).WithMessage("BrandId не может быть Guid.Empty");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId не может быть пустым")
            .NotEqual(Guid.Empty).WithMessage("CategoryId не может быть Guid.Empty");
    }       
}
