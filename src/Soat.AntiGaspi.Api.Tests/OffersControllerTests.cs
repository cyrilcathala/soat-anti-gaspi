using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Soat.AntiGaspi.Api.Contracts;
using Soat.AntiGaspi.Api.Models;
using Soat.AntiGaspi.Api.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Soat.AntiGaspi.Api.Tests
{
    public class OffersControllerTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly Fixture _fixture;

        public OffersControllerTests(ApiWebApplicationFactory fixture)
        {
            _httpClient = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(ApiWebApplicationFactory.ApiUrl)
            });

            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_Should_ReturnCollection_When_OK()
        {
            var createOfferRequest = _fixture.Build<CreateOfferRequest>()
                                             .With(c => c.Email, "toto@toto.fr")
                                             .With(c => c.Availability, DateTime.UtcNow)
                                             .With(c => c.Expiration, DateTime.UtcNow.AddDays(42))
                                             .Create();

            var createOfferResponse = await _httpClient.PostAsync("/api/offers", createOfferRequest);

            var offerId = await createOfferResponse.ReadAsObject<string>();

            var confirmResponse = await _httpClient.PostAsync($"/api/offers/{offerId}/confirm", string.Empty);

            var httpResponse = await _httpClient.GetAsync("/api/offers");

            var offers = await httpResponse.ReadAsObject<GetOffersResponse>();

            Assert.True(httpResponse.IsSuccessStatusCode);
            Assert.NotEmpty(offers.Items);
        }

        [Fact]
        public async Task Should_CreateAnOffer_When_CreationIsOK()
        {
            var expectedTitle = "This is a title";

            var createOfferRequest = _fixture.Build<CreateOfferRequest>()
                .With(c => c.Title, expectedTitle)
                .With(c => c.Email, "toto@toto.fr")
                .With(c => c.Availability, DateTime.UtcNow)
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

        [Fact]
        public async Task Should_DeleteOffer_When_DeletionIsOK()
        {
            var expectedTitle = "This is a title";

            var createOfferRequest = _fixture.Build<CreateOfferRequest>()
                .With(c => c.Title, expectedTitle)
                .With(c => c.Email, "toto@toto.fr")
                .With(c => c.Availability, DateTime.UtcNow)
                .With(c => c.Expiration, DateTime.UtcNow.AddDays(42))
                .Create();

            var createOfferResponse = await _httpClient.PostAsync("/api/offers", createOfferRequest);

            var offerId = await createOfferResponse.ReadAsObject<string>();

            var deleteOfferResponse = await _httpClient.DeleteAsync($"/api/offers/{offerId}");

            Assert.True(deleteOfferResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Should_DeleteOffer_ReturnNotFound_When_OfferNotExist()
        {
            var nonExistingGuid = Guid.NewGuid();

            var deleteOfferResponse = await _httpClient.DeleteAsync($"/api/offers/{nonExistingGuid}");

            Assert.Equal(HttpStatusCode.NotFound, deleteOfferResponse.StatusCode);
        }

        [Fact]
        public async Task Should_ConfirmOffer_When_ConfirmationIsOK()
        {
            var createOfferRequest = _fixture.Build<CreateOfferRequest>()
                                             .With(c => c.Email, "toto@toto.fr")
                                             .With(c => c.Availability, DateTime.UtcNow)
                                             .With(c => c.Expiration, DateTime.UtcNow.AddDays(42))
                                             .Create();

            var createOfferResponse = await _httpClient.PostAsync("/api/offers", createOfferRequest);

            var offerId = await createOfferResponse.ReadAsObject<string>();
            var location = createOfferResponse.Headers.Location;

            var confirmResponse = await _httpClient.PostAsync($"/api/offers/{offerId}/confirm", string.Empty);

            Assert.True(confirmResponse.IsSuccessStatusCode);
        }
    }
}