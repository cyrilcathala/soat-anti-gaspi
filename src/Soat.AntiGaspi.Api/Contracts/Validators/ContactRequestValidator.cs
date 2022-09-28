using FluentValidation;

namespace Soat.AntiGaspi.Api.Contracts.Validators;

public class ContactRequestValidator : AbstractValidator<ContactRequest>
{
    public ContactRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x)
            .Must(HavePhoneOrEmail);

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Phone)
            .Matches("0[1-9][0-9]{8}$");
    }

    private bool HavePhoneOrEmail(ContactRequest contact)
    {
        return !string.IsNullOrWhiteSpace(contact.Phone)
            || !string.IsNullOrWhiteSpace(contact.Email);
    }
}
