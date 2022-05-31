using AutoFixture;
using Newtonsoft.Json;
using Soat.AntiGaspi.Api.Contracts;
using Soat.AntiGaspi.Api.Models;
using Soat.AntiGaspi.Api.Tests.Extensions;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
    }
}