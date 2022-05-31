using FluentValidation;

namespace Soat.AntiGaspi.Api.Contracts.Validators;

public class CreateAnnonceRequestValidator : AbstractValidator<CreateAnnonceRequest>
{
    public CreateAnnonceRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.CompanyName)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.Availability)
            .NotNull();

        RuleFor(x => x.Expiration)
            .NotNull();
    }
}
