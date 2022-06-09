using AutoFixture;
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
    public class AnnonceControllerTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly Fixture _fixture;

        public AnnonceControllerTests(ApiWebApplicationFactory fixture)
        {
            _httpClient = fixture.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(ApiWebApplicationFactory.ApiUrl)
            });

            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_Should_ReturnCollection_When_OK()
        {
            var httpResponse = await _httpClient.GetAsync("/api/annonces");

            var content = await httpResponse.Content.ReadAsStringAsync();
            var annonces = JsonConvert.DeserializeObject<IEnumerable<Annonce>>(content);

            Assert.True(httpResponse.IsSuccessStatusCode);
            Assert.NotEmpty(annonces);
        }

        [Fact]
        public async Task Should_SendAnEmailToConfirmAnAnnonceCreation_When_AnnonceIsOK()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_CreateAnAnnonce_When_CreationIsOK()
        {
            var createAnnonceRequest = _fixture.Build<CreateAnnonceRequest>()
                                               .With(c => c.Email, "toto@toto.fr")
                                               .With(c => c.Availability, DateTime.UtcNow)
                                               .With(c => c.Expiration, DateTime.UtcNow.AddDays(42))
                                               .Create();

            var createAnnonceResponse = await _httpClient.PostAsync("/api/annonces", createAnnonceRequest);

            var location = createAnnonceResponse.Headers.Location;

            var getAnnonceResponse = await _httpClient.GetAsync(location);
            var createdAnnonce = await getAnnonceResponse.ReadAsObject<Annonce>();

            Assert.True(createAnnonceResponse.IsSuccessStatusCode);
            Assert.NotNull(createdAnnonce);
            Assert.Equal(createAnnonceRequest.Email, createdAnnonce.Email);
        }

        [Fact]
        public async Task Should_DeleteAnnonce_When_DeletionIsOK()
        {
            var deleteAnnonceResponse = await _httpClient.DeleteAsync("/api/annonces/463e5d9f-5998-456d-8c91-fa522f2a2469");
            Assert.True(deleteAnnonceResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Should_DeleteAnnonce_ReturnNotFound_When_AnnonceNotExist()
        {
            var deleteAnnonceResponse = await _httpClient.DeleteAsync("/api/annonces/078c6290-b80b-4157-9758-de4af6705f4d");
            Assert.Equal(HttpStatusCode.NotFound, deleteAnnonceResponse.StatusCode);
        }

        [Fact]
        public async Task Should_ConfirmAnnonce_When_ConfirmationIsOK()
        {
            var confirmAnnonceResponse = await _httpClient.PostAsync("/api/annonces/7675d27b-241c-48a9-aa03-4047095ec353/confirm", null);
            Assert.True(confirmAnnonceResponse.IsSuccessStatusCode);
            var confirmedAnnonce = await confirmAnnonceResponse.ReadAsObject<Annonce>();
            Assert.NotNull(confirmedAnnonce);
            Assert.Equal(AnnonceStatus.Active, confirmedAnnonce.Status);
        }
    }
}