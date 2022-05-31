using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Soat.AntiGaspi.Api.Repository;

namespace Soat.AntiGaspi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddControllers()
            .AddFluentValidation(s => s.RegisterValidatorsFromAssemblyContaining<Program>());

        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<AntiGaspiContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("AntiGaspiContext")));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}