using FluentValidation;

namespace Soat.AntiGaspi.Api.Contracts.Validators;

public class CreateOfferRequestValidator : AbstractValidator<CreateOfferRequest>
{
    public CreateOfferRequestValidator()
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
            .NotNull()
            .Must(availability => DateTimeOffset.UtcNow.Date <= availability)
            .WithMessage("Availability cannot be in the past.");

        RuleFor(x => x.Expiration)
            .NotNull();

        RuleFor(x => x)
            .Must(ExpirationBeforeAvailability)
            .WithMessage("The offer should expire after the availability date.");
    }

    private bool ExpirationBeforeAvailability(CreateOfferRequest offer) 
        => offer.Availability < offer.Expiration;
}
