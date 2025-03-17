using FluentValidation;
using MyApiV8.Application.DTOs;

namespace MyApiV8.Application.Validations.FluentValidation;

public class TokenValidation : AbstractValidator<TokenDTO>
{
    public TokenValidation()
    {
        RuleFor(c => c.RefreshToken)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.");
    }
}