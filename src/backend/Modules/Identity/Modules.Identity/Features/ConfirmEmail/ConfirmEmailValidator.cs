using System.Text.RegularExpressions;
using FluentValidation;

namespace Modules.Identity.Features.ConfirmEmail;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email не может быть пустым")
            .MaximumLength(100).WithMessage("Email не может быть больше 100 символов")
            .EmailAddress().WithMessage("Неверный формат email")
            .Must(email =>
                email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase) ||
                email.EndsWith("@yandex.ru", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Разрешены только Gmail и Yandex");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code не может быть пустым")
            .Length(5).WithMessage("Code может содержать 5 символов");
    }
}