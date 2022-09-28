using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Soat.AntiGaspi.Api.Contracts;
using Soat.AntiGaspi.Api.Controllers;
using Soat.AntiGaspi.Api.Tests.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Soat.AntiGaspi.Api.Tests
{
    public class OffersControllerTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly Fixture _fixture;

        public OffersControllerTests(ApiWebApplicationFactory webAppFactory)
        {
            _httpClient = webAppFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(ApiWebApplicationFactory.ApiUrl)
            });

            _fixture = new Fixture();
        }

        [Fact]
        [Trait(nameof(OffersController.Create), "Success")]
        public async Task Should_CreateAnOffer_When_CreationIsOK()
        {
            var expectedTitle = "This is a title";

            var createOfferRequest = _fixture.Build<CreateOfferRequest>()
                .With(c => c.Title, expectedTitle)
                .With(c => c.Email, "toto@toto.fr")
                .With(c => c.Availability, DateTime.UtcNow.AddDays(1))
                .With(c => c.Expiration, DateTime.UtcNow.AddDays(42))
                .Create();

            var createOfferResponse = await _httpClient.PostAsync("/api/offers", createOfferRequest);

            var location = createOfferResponse.Headers.Location;

            var getOfferResponse = await _httpClient.GetAsync(location);
            var createdOffer = await getOfferResponse.ReadAsObject<GetOfferResponse>();

            Assert.True(createOfferResponse.IsSuccessStatusCode);
            Assert.NotNull(createdOffer);
            Assert.Equal(expectedTitle, createdOffer.Title);
        }

        private async Task<string> CreateOffer()
        {
            var createOfferRequest = _fixture
                .Build<CreateOfferRequest>()
                .With(c => c.Email, "toto@toto.fr")
                .With(c => c.Availability, DateTime.UtcNow.AddDays(1))
                .With(c => c.Expiration, DateTime.UtcNow.AddDays(42))
                .Create();

            var createOfferResponse = await _httpClient.PostAsync("/api/offers", createOfferRequest);

            return await createOfferResponse.ReadAsObject<string>();
        }
    }
}