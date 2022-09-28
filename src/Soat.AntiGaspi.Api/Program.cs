using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Soat.AntiGaspi.Api.Repository;
using Soat.AntiGaspi.Api.Time;

namespace Soat.AntiGaspi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddApplicationInsightsTelemetry();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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