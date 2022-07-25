using Microsoft.EntityFrameworkCore;
using Sgbj.Cron;
using Soat.AntiGaspi.Api.Repository;

namespace Soat.AntiGaspi.Api.BackgroundJobs;
public class CleanContactsJob : BackgroundService
{
    private readonly AntiGaspiContext _antiGaspiContext;
    private const int CleanExpirationInDays = 30;

    public CleanContactsJob(AntiGaspiContext antiGaspiContext)
    {
        _antiGaspiContext = antiGaspiContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    { 
        using var timer = new CronTimer("0 6 * * *", TimeZoneInfo.Local);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await CleanContacts();
        }
    }

    private Task CleanContacts()
    {
        var oldContacts = _antiGaspiContext.ContactOffers
            .Where(contact => contact.CreationDate < DateTimeOffset.Now.AddDays(-CleanExpirationInDays));

        foreach (var contact in oldContacts)
        {
            _antiGaspiContext.Remove(contact);
        }

        return _antiGaspiContext.SaveChangesAsync();
    }
}
