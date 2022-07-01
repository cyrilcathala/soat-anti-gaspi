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
            .Must(availability => DateTime.UtcNow.Date <= availability);

        RuleFor(x => x.Expiration)
            .NotNull();

        RuleFor(x => x)
            .Must(ExpirationBeforeAvailability);
    }

    private bool ExpirationBeforeAvailability(CreateOfferRequest offer) 
        => offer.Availability < offer.Expiration;
}
