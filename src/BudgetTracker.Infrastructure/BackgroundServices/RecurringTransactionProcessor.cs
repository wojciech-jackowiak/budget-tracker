using BudgetTracker.Domain.Entities;
using BudgetTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace BudgetTracker.Infrastructure.BackgroundServices
{
    public class RecurringTransactionProcessor : BackgroundService
    {
        private readonly ILogger<RecurringTransactionProcessor> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RecurringTransactionProcessor(
            ILogger<RecurringTransactionProcessor> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RecurringTransactionProcessor starting...");

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Processing recurring transactions...");

                    await ProcessRecurringTransactions(stoppingToken);

                    _logger.LogInformation("Recurring transactions processed successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing recurring transactions: {Message}", ex.Message);
                }

                // DEVELOPMENT: Every 1 min
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                // PRODUCTION: Every 24h 
                //await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            _logger.LogInformation("RecurringTransactionProcessor stopping...");
        }

        private async Task ProcessRecurringTransactions(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var today = DateTime.UtcNow;
            var currentMonth = today.ToString("yyyy-MM");

            _logger.LogInformation("Current date: {Date}, Month: {Month}", today.Date, currentMonth);

            var recurringTransactions = await context.RecurringTransactions
                .Include(r => r.Category)
                .Where(r => r.IsActive)
                .Where(r => r.StartDate <= today)
                .Where(r => r.EndDate == null || r.EndDate >= today)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} active recurring transactions", recurringTransactions.Count);

            int processedCount = 0;

            foreach (var recurring in recurringTransactions)
            {
                try
                {
                    if (recurring.ShouldProcessForMonth(currentMonth, today))
                    {
                        _logger.LogInformation(
                            "Creating transaction for recurring #{Id}: {Description} ({Amount})",
                            recurring.Id,
                            recurring.Description,
                            recurring.Amount
                        );

                        var transaction = Transaction.Create(
                            userId: recurring.UserId,
                            categoryId: recurring.CategoryId,
                            amount: recurring.Amount,
                            type: recurring.Type,
                            description: recurring.Description,
                            date: today,
                            isFromRecurring: true,
                            recurringTransactionId: recurring.Id
                        );

                        context.Transactions.Add(transaction);

                        recurring.MarkAsProcessed(currentMonth);

                        processedCount++;
                    }
                    else
                    {
                        _logger.LogDebug(
                            "Skipping recurring #{Id}: Not due yet or already processed",
                            recurring.Id
                        );
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error processing recurring transaction #{Id}: {Message}",
                        recurring.Id,
                        ex.Message
                    );
                }
            }

            if (processedCount > 0)
            {
                await context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Created {Count} transactions from recurring", processedCount);
            }
            else
            {
                _logger.LogInformation("No recurring transactions needed processing");
            }
        }
    }
}
