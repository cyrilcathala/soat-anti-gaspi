using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Soat.AntiGaspi.Api.BackgroundJobs;
using Soat.AntiGaspi.Api.Constants;
using Soat.AntiGaspi.Api.Contracts;
using Soat.AntiGaspi.Api.Repository;
using Soat.AntiGaspi.Api.Tests.Extensions;
using Soat.AntiGaspi.Api.Tests.Fakes;
using Soat.AntiGaspi.Api.Time;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Soat.AntiGaspi.Api.Tests;

public class CleanContactsJobTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly Fixture _fixture;
    private readonly DateTimeOffsetFake _dateTimeOffsetFake;

    public CleanContactsJobTests(ApiWebApplicationFactory webAppFactory)
    {
        _dateTimeOffsetFake = new DateTimeOffsetFake();
        webAppFactory.Configure(
            services => services.AddSingleton<IDateTimeOffset>(_dateTimeOffsetFake),
            configurationBuilder => configurationBuilder.Properties.Add(AppSettingKeys.CleanContactsTimer, "*/1 * * * *"));

        _httpClient = webAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(ApiWebApplicationFactory.ApiUrl)
        });

        _serviceProvider = webAppFactory.Services;
        _fixture = new Fixture();
    }

    [Fact]
    [Trait(nameof(CleanContactsJob), "Success")]
    public async Task Should_CleanContacts_When_OlderThan30Days()
    {
        var offerId = await CreateContact();
        _dateTimeOffsetFake.Now = DateTimeOffset.UtcNow.AddDays(35);

        await Task.Delay(TimeSpan.FromMinutes(1));

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AntiGaspiContext>();
        var contactOffer = context.ContactOffers
            .Where(contact => contact.OfferId == offerId)
            .FirstOrDefault();

        Assert.Null(contactOffer);
    }

    private async Task<Guid> CreateContact()
    {
        var createOfferRequest = _fixture
            .Build<CreateOfferRequest>()
            .With(c => c.Email, "toto@toto.fr")
            .With(c => c.Availability, DateTimeOffset.UtcNow)
            .With(c => c.Expiration, DateTimeOffset.UtcNow.AddDays(42))
            .Create();

        var createOfferResponse = await _httpClient.PostAsync("/api/offers", createOfferRequest);

        var offerId = await createOfferResponse.ReadAsObject<Guid>();

        await _httpClient.PostAsync($"/api/offers/{offerId}/confirm", string.Empty);

        var contactRequest = _fixture
           .Build<ContactRequest>()
           .With(c => c.Email, "toto@toto.fr")
           .With(c => c.Phone, (string?)null)
           .Create();

        var contactResponse = await _httpClient.PostAsync($"/api/offers/{offerId}/contact", contactRequest);

        return offerId;
    }
}
