﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SendGrid;
using System;

namespace Soat.AntiGaspi.Api.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    internal const string ApiUrl = "https://localhost:7201";
    private Action<IServiceCollection> _configureServices;
    private Action<IConfigurationBuilder> _configureBuilder;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(ConfigureApp);
        builder.ConfigureServices(ConfigureServices);
        builder.UseUrls(ApiUrl);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped(s => new Mock<ISendGridClient>().Object);

        _configureServices?.Invoke(services);
    }

    private void ConfigureApp(
        WebHostBuilderContext hostContext,
        IConfigurationBuilder configurationBuilder)
    {
        _configureBuilder?.Invoke(configurationBuilder);
    }

    public void Configure(
        Action<IServiceCollection> configureServices,
        Action<IConfigurationBuilder> configureBuilder)
    {
        _configureServices = configureServices;
        _configureBuilder = configureBuilder;
    }
}