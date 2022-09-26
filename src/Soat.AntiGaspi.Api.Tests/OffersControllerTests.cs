using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Soat.AntiGaspi.Api.Contracts;
using Soat.AntiGaspi.Api.Controllers;
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

        public OffersControllerTests(ApiWebApplicationFactory webAppFactory)
        {
            _httpClient = webAppFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(ApiWebApplicationFactory.ApiUrl)
            });

            _fixture = new Fixture();
        }

        [Fact]
        [Trait(nameof(OffersController.Get), "Success")]
        public async Task Get_Should_ReturnCollection_When_OK()
        {
            var offerId = await CreateOffer();

            await _httpClient.PostAsync($"/api/offers/{offerId}/confirm", string.Empty);

            var httpResponse = await _httpClient.GetAsync("/api/offers");

            var offers = await httpResponse.ReadAsObject<GetOffersResponse>();

            Assert.True(httpResponse.IsSuccessStatusCode);
            Assert.NotEmpty(offers.Items);
        }

        [Fact]
        [Trait(nameof(OffersController.Create), "Success")]
        public async Task Should_CreateAnOffer_When_CreationIsOK()
        {
            var expectedTitle = "This is a title";

            var createOfferRequest = _fixture.Build<CreateOfferRequest>()
                .With(c => c.Title, expectedTitle)
                .With(c => c.Email, "toto@toto.fr")
                .With(c => c.Availability, DateOnly.FromDateTime(DateTime.UtcNow))
                .With(c => c.Expiration, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(42)))
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
        [Trait(nameof(OffersController.Delete), "Success")]
        public async Task Should_DeleteOffer_When_DeletionIsOK()
        {
            var offerId = await CreateOffer();

            var deleteOfferResponse = await _httpClient.DeleteAsync($"/api/offers/{offerId}");

            Assert.True(deleteOfferResponse.IsSuccessStatusCode);
        }

        [Fact]
        [Trait(nameof(OffersController.Delete), "Not Found")]
        public async Task Should_DeleteOffer_ReturnNotFound_When_OfferNotExist()
        {
            var nonExistingGuid = Guid.NewGuid();

            var deleteOfferResponse = await _httpClient.DeleteAsync($"/api/offers/{nonExistingGuid}");

            Assert.Equal(HttpStatusCode.NotFound, deleteOfferResponse.StatusCode);
        }

        [Fact]
        [Trait(nameof(OffersController.Confirm), "Success")]
        public async Task Should_ConfirmOffer_When_ConfirmationIsOK()
        {
            var offerId = await CreateOffer();

            var confirmResponse = await _httpClient.PostAsync($"/api/offers/{offerId}/confirm", string.Empty);

            Assert.True(confirmResponse.IsSuccessStatusCode);
        }

        [Fact]
        [Trait(nameof(OffersController.Contact), "Success")]
        public async Task Should_ContactOffer_When_ContactRequestIsOK()
        {
            var offerId = await CreateOffer();

            await _httpClient.PostAsync($"/api/offers/{offerId}/confirm", string.Empty);

            var contactRequest = _fixture
               .Build<ContactRequest>()
               .With(c => c.Email, "toto@toto.fr")
               .With(c => c.Phone, (string?)null)
               .Create();

            var contactResponse = await _httpClient.PostAsync($"/api/offers/{offerId}/contact", contactRequest);

            Assert.True(contactResponse.IsSuccessStatusCode);
        }

        private async Task<string> CreateOffer()
        {
            var createOfferRequest = _fixture
                .Build<CreateOfferRequest>()
                .With(c => c.Email, "toto@toto.fr")
                .With(c => c.Availability, DateOnly.FromDateTime(DateTime.UtcNow))
                .With(c => c.Expiration, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(42)))
                .Create();

            var createOfferResponse = await _httpClient.PostAsync("/api/offers", createOfferRequest);

            return await createOfferResponse.ReadAsObject<string>();
        }
    }
}