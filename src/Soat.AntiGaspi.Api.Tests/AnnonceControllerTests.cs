using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Xunit;

namespace Soat.AntiGaspi.Api.Tests
{
    public class AnnonceControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AnnonceControllerTests()
        {
            //_server = new TestServer(new WebHostBuilder().UseStartup<Program>());
            //_client = _server.CreateClient();
        }

        [Fact]
        public void Should_get_annonce_when_id_exists()
        {

        }
    }
}