using FluentValidation;
using MyApiV8.Application.DTOs;

namespace MyApiV8.Application.Validations.FluentValidation;

public class RegisterUserValidation : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserValidation()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.")
            .Length(2, 200)
            .WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.")
            .Length(6, 100)
            .WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters.");

        RuleFor(c => c.ConfirmPassword)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.")
            .Equal(c => c.Password)
            .WithMessage("Please make sure your passwords match.");
    }
}