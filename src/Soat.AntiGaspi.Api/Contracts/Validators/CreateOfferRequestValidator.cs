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
            .Must(availability => availability == null || DateTime.UtcNow <= availability.GetValueOrDefault())
            .WithMessage("Availability cannot be in the past.");

        RuleFor(x => x)
            .Must(ExpirationBeforeAvailability)
            .WithMessage("The offer should expire after the availability date.");
    }

    private bool ExpirationBeforeAvailability(CreateOfferRequest offer) 
        => offer.Availability == null 
        || offer.Expiration == null 
        || offer.Availability < offer.Expiration;
}
