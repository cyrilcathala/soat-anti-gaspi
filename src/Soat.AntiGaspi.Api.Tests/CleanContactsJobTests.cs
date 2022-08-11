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
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Soat.AntiGaspi.Api.Tests;

public class CleanContactsJobTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly Fixture _fixture;
    private readonly DateOnlyFake _dateTimeFake;

    public CleanContactsJobTests(ApiWebApplicationFactory webAppFactory)
    {
        _dateTimeFake = new DateOnlyFake();
        
        _httpClient = webAppFactory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            [AppSettingKeys.CleanContactsTimer] = "*/10 * * * * *"
                        });
                });

                builder.ConfigureServices(
                    services => services.AddSingleton<IDateOnly>(_dateTimeFake));
            })
            .CreateClient(new WebApplicationFactoryClientOptions
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
        _dateTimeFake.Now = DateTime.UtcNow.AddDays(35);

        await Task.Delay(TimeSpan.FromSeconds(30));

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
            .With(c => c.Availability, DateTime.UtcNow)
            .With(c => c.Expiration, DateTime.UtcNow.AddDays(42))
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
