using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SendGrid;

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
        services.AddScoped(s => new Mock<ISendGridClient>().Object);
    }

    private void ConfigureApp(
        WebHostBuilderContext hostContext,
        IConfigurationBuilder configurationBuilder)
    {
    }
}