using FluentValidation;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Enums;
using MyApiV8.Domain.Validations.DocumentosValidation;

namespace MyApiV8.Domain.Validations.FluentValidation;

public class SupplierValidation : AbstractValidator<Supplier>
{
    public SupplierValidation()
    {
        RuleFor(f => f.Name)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        When(f => f.SupplierType == SupplierTypeEnum.NaturalPerson, () =>
        {
            RuleFor(f => f.Document.Length).Equal(CpfValidation.MaxLength)
                .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
            RuleFor(f => CpfValidation.Validar(f.Document)).Equal(true)
                .WithMessage("O documento fornecido é inválido.");
        });

        When(f => f.SupplierType == SupplierTypeEnum.LegalPerson, () =>
        {
            RuleFor(f => f.Document.Length).Equal(CnpjValidation.MaxLength)
                .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
            RuleFor(f => CnpjValidation.Validar(f.Document)).Equal(true)
                .WithMessage("O documento fornecido é inválido.");
        });
    }
}