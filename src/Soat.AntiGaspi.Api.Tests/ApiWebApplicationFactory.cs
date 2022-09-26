using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Soat.AntiGaspi.Api.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    internal const string ApiUrl = "https://localhost:7201";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(ConfigureApp);
        builder.ConfigureServices(ConfigureServices);
        builder.UseUrls(ApiUrl);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var sendGridMock = new Mock<ISendGridClient>();
        sendGridMock
            .Setup(s => s.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response(HttpStatusCode.Accepted, new StringContent("Mock"), null));

        services.AddScoped(s => sendGridMock.Object);
    }

    private void ConfigureApp(
        WebHostBuilderContext hostContext,
        IConfigurationBuilder configurationBuilder)
    {
    }
}