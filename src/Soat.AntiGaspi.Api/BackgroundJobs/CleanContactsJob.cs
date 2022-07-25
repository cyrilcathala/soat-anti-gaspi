using Cronos;
using Sgbj.Cron;
using Soat.AntiGaspi.Api.Constants;
using Soat.AntiGaspi.Api.Repository;
using Soat.AntiGaspi.Api.Time;

namespace Soat.AntiGaspi.Api.BackgroundJobs;
public class CleanContactsJob : BackgroundService
{
    private const int CleanExpirationInDays = 30;

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDateTimeOffset _dateTimeOffset;
    private readonly string _cleanContactsTimer;

    public CleanContactsJob(
        IServiceScopeFactory serviceScopeFactory,        
        IConfiguration configuration,
        IDateTimeOffset dateTimeOffset)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _dateTimeOffset = dateTimeOffset;
        _cleanContactsTimer = configuration.GetValue<string>(AppSettingKeys.CleanContactsTimer);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new CronTimer(CronExpression.Parse(_cleanContactsTimer, CronFormat.IncludeSeconds), TimeZoneInfo.Local);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await CleanContacts();
        }
    }

    private Task CleanContacts()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AntiGaspiContext>();

        var oldContacts = context.ContactOffers
            .Where(contact => contact.CreationDate < _dateTimeOffset.Now.AddDays(-CleanExpirationInDays));

        foreach (var contact in oldContacts)
        {
            context.Remove(contact);
        }

        return context.SaveChangesAsync();
    }
}
