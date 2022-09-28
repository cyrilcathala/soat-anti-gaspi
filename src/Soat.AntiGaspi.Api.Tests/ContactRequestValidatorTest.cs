using FluentValidation.TestHelper;
using Soat.AntiGaspi.Api.Contracts.Validators;
using Xunit;

namespace Soat.AntiGaspi.Api.Tests;
public class ContactRequestValidatorTest
{
    private readonly ContactRequestValidator _validator;

    public ContactRequestValidatorTest()
    {
        _validator = new ContactRequestValidator();
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.FirstName), "Error")]
    public void Should_Fail_When_FirstNameIsEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    FirstName = string.Empty
                })
            .ShouldHaveValidationErrorFor(c => c.FirstName);
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.LastName), "Error")]
    public void Should_Fail_When_LastNameIsEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    LastName = string.Empty
                })
            .ShouldHaveValidationErrorFor(c => c.LastName);
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.Email), "Error")]
    public void Should_Fail_When_EmailIsEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    Email = string.Empty
                })
            .ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.Email), "Error")]
    public void Should_Fail_When_EmailIsNotAValidEmail()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    Email = "Not an email"
                })
            .ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.Phone), "Error")]
    public void Should_Fail_When_PhoneIsNotAValidPhoneNumber()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    Phone = "I am not a phone number"
                })
            .ShouldHaveValidationErrorFor(c => c.Phone);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData("", " ")]
    [Trait(nameof(Contracts.ContactRequest), "Error")]
    public void Should_Fail_When_PhoneAndEmailAreEmpty(string phone, string email)
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    Phone = phone,
                    Email = email
                })
            .ShouldHaveValidationErrorFor(c => c);
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.FirstName), "Valid case")]
    public void Should_Succeed_When_FirstNameIsNotEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    FirstName = "James"
                })
            .ShouldNotHaveValidationErrorFor(c => c.FirstName);
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.LastName), "Valid case")]
    public void Should_Succeed_When_LastNameIsNotEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    LastName = "Bond"
                })
            .ShouldNotHaveValidationErrorFor(c => c.LastName);
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.Email), "Valid case")]
    public void Should_Succeed_When_EmailIsAValidEmail()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    Email = "james.bond@mi6.co.uk"
                })
            .ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    [Trait(nameof(Contracts.ContactRequest.Phone), "Valid case")]
    public void Should_Succeed_When_PhoneIsAValidPhoneNumber()
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    Phone = "0123456789"
                })
            .ShouldNotHaveValidationErrorFor(c => c.Phone);
    }

    [Theory]
    [InlineData(null, "james.bond@mi6.co.uk")]
    [InlineData("", "james.bond@mi6.co.uk")]
    [InlineData(" ", "james.bond@mi6.co.uk")]
    [InlineData("0123456789", null)]
    [InlineData("0123456789", "")]
    [InlineData("0123456789", " ")]
    [Trait(nameof(Contracts.ContactRequest), "Valid case")]
    public void Should_Succeed_When_PhoneAndEmailAreEmpty(string phone, string email)
    {
        _validator
            .TestValidate(
                new Contracts.ContactRequest
                {
                    Phone = phone,
                    Email = email
                })
            .ShouldNotHaveValidationErrorFor(c => c);
    }
}
