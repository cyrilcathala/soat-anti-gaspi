using FluentValidation.TestHelper;
using Soat.AntiGaspi.Api.Contracts.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Soat.AntiGaspi.Api.Tests;
public class CreateOfferRequestValidatorTest
{
    private readonly CreateOfferRequestValidator _validator;

    public CreateOfferRequestValidatorTest()
    {
        _validator = new CreateOfferRequestValidator();
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Email), "Error")]
    public void Should_Fail_When_EmailIsEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Email = string.Empty
                })
            .ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Email), "Error")]
    public void Should_Fail_When_EmaiIsNotAValidEmail()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Email = "NotAnEmail"
                })
            .ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Title), "Error")]
    public void Should_Fail_When_TitleIsEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Title = string.Empty
                })
            .ShouldHaveValidationErrorFor(c => c.Title);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Description), "Error")]
    public void Should_Fail_When_DescriptionIsEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Description = string.Empty
                })
            .ShouldHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.CompanyName), "Error")]
    public void Should_Fail_When_CompanyNameIsEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    CompanyName = string.Empty
                })
            .ShouldHaveValidationErrorFor(c => c.CompanyName);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Address), "Error")]
    public void Should_Fail_When_AddressIsEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Address = string.Empty
                })
            .ShouldHaveValidationErrorFor(c => c.Address);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Availability), "Error")]
    public void Should_Fail_When_AvailabilityIsNull()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Availability = null
                })
            .ShouldHaveValidationErrorFor(c => c.Availability);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Availability), "Error")]
    public void Should_Fail_When_AvailabilityIsInThePast()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Availability = DateTimeOffset.UtcNow.AddDays(-5)
                })
            .ShouldHaveValidationErrorFor(c => c.Availability);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Expiration), "Error")]
    public void Should_Fail_When_ExpirationIsNull()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Expiration = null
                })
            .ShouldHaveValidationErrorFor(c => c.Expiration);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest), "Error")]
    public void Should_Fail_When_ExpirationIsBeforeAvailability()
    {
        var availability = DateTimeOffset.UtcNow;
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Availability = availability,
                    Expiration = availability.AddDays(-1)
                })
            .ShouldHaveValidationErrorFor(c => c);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Email), "Valid case")]
    public void Should_Succeed_When_EmailIsNotEmptyAndValid()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Email = "coucou@pouet.com"
                })
            .ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Title), "Valid case")]
    public void Should_Succeed_When_TitleIsNotEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Title = "I'm a title 😜"
                })
            .ShouldNotHaveValidationErrorFor(c => c.Title);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Description), "Valid case")]
    public void Should_Succeed_When_DescriptionIsNotEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Description = "I'm an awesome description 😎"
                })
            .ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.CompanyName), "Valid case")]
    public void Should_Succeed_When_CompanyNameIsNotEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    CompanyName = "Better than Facebook 🚀"
                })
            .ShouldNotHaveValidationErrorFor(c => c.CompanyName);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Address), "Valid case")]
    public void Should_Succeed_When_AddressIsNotEmpty()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Address = "3 rue de l'impasse 75000 PerduVille"
                })
            .ShouldNotHaveValidationErrorFor(c => c.Address);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Availability), "Valid case")]
    public void Should_Succeed_When_AvailabilityIsNotNull()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Availability = DateTimeOffset.UtcNow
                })
            .ShouldNotHaveValidationErrorFor(c => c.Availability);
    }

    [Fact]
    [Trait(nameof(Contracts.CreateOfferRequest.Expiration), "Valid case")]
    public void Should_Succeed_When_ExpirationIsNotNull()
    {
        _validator
            .TestValidate(
                new Contracts.CreateOfferRequest
                {
                    Expiration = DateTimeOffset.UtcNow
                })
            .ShouldNotHaveValidationErrorFor(c => c.Expiration);
    }
}
