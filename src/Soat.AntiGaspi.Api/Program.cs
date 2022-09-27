using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using SendGrid.Extensions.DependencyInjection;
using Soat.AntiGaspi.Api.BackgroundJobs;
using Soat.AntiGaspi.Api.Constants;
using Soat.AntiGaspi.Api.Repository;
using Soat.AntiGaspi.Api.Time;
using System.Globalization;

namespace Soat.AntiGaspi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddControllers()
            .AddFluentValidation(s => s.RegisterValidatorsFromAssemblyContaining<Program>())
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

        builder.Services.AddSingleton<IDateOnly, DateOnlyProvider>();

        builder.Services.AddApplicationInsightsTelemetry();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSendGrid(options => options.ApiKey = builder.Configuration[AppSettingKeys.SendGridApiKey]);

        builder.Services.AddHostedService<CleanContactsJob>();

        builder.Services.AddDbContext<AntiGaspiContext>(options =>
            options.UseNpgsql(builder.Configuration["POSTGRESQLCONNSTR_AntiGaspi"]));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("dev", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        if (app.Environment.IsDevelopment())
        {
            app.UseCors("dev");
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}