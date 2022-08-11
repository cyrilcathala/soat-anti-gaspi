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
    private readonly IDateOnly _dateTime;
    private readonly ILogger<CleanContactsJob> _logger;
    private readonly string _cleanContactsTimer;

    public CleanContactsJob(
        IServiceScopeFactory serviceScopeFactory,        
        IConfiguration configuration,
        IDateOnly dateTime,
        ILogger<CleanContactsJob> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _dateTime = dateTime;
        _logger = logger;
        _cleanContactsTimer = configuration.GetValue<string>(AppSettingKeys.CleanContactsTimer);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new CronTimer(
            CronExpression.Parse(_cleanContactsTimer, CronFormat.IncludeSeconds), TimeZoneInfo.Local);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await CleanContacts();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
            }
        }
    }

    private async Task CleanContacts()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AntiGaspiContext>();

        var oldContacts = context.ContactOffers
            .Where(contact => contact.CreationDate < _dateTime.Now.AddDays(-CleanExpirationInDays).ToDateTime(TimeOnly.Parse("00:00 AM")));

        foreach (var contact in oldContacts)
        {
            context.Remove(contact);
        }

        await context.SaveChangesAsync();
    }
}
