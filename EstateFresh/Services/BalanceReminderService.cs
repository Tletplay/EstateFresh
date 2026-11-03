using EstateFresh.Data;
using Microsoft.EntityFrameworkCore;

namespace EstateFresh.Services;

public class BalanceReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BalanceReminderService> _logger;

    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan ReminderGap = TimeSpan.FromHours(24);
    private const int MaxReminders = 3;

    public BalanceReminderService(IServiceScopeFactory scopeFactory, ILogger<BalanceReminderService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendDueRemindersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Balance reminder check failed.");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    private async Task SendDueRemindersAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var candidates = await db.Orders
            .Include(o => o.Customer)
            .Where(o => o.Status == "Delivered"
                     && !o.BalancePaid
                     && o.BalanceDue.HasValue && o.BalanceDue.Value > 0
                     && o.BalanceReminderCount < MaxReminders
                     && o.DeliveredAt.HasValue)
            .ToListAsync();

        foreach (var order in candidates)
        {
            var baseline = order.LastReminderSentAt ?? order.DeliveredAt!.Value;
            if (DateTime.UtcNow - baseline < ReminderGap)
                continue;

            if (order.Customer == null) continue;

            var emailBody = $@"
                <h2>Balance Due Reminder</h2>
                <p>Hi {order.Customer.FullName},</p>
                <p>Your laundry from Order #{order.OrderID} was delivered, and a balance of R{order.BalanceDue:0.00} is still outstanding.</p>
                <p>Please log in to Estate Fresh and pay your balance to keep your account in good standing.</p>
                <hr />
                <small>Estate Fresh - Premium Laundry Concierge</small>
            ";

            await emailService.SendEmailAsync(order.Customer.Email, "Balance Due Reminder - Estate Fresh", emailBody);

            order.BalanceReminderCount += 1;
            order.LastReminderSentAt = DateTime.UtcNow;
        }

        if (candidates.Any())
        {
            await db.SaveChangesAsync();
        }
    }
}