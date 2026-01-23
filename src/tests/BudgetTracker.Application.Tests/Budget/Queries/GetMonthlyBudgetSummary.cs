using BudgetTracker.Application.Budget.Queries.GetMonthlyBudgetSummary;
using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using BudgetTracker.Infrastructure.Data;
using FluentAssertions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Application.Tests.Budget.Queries
{
    public class GetMonthlyBudgetSummaryHandlerTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            // Seed categories
            var categories = new[]
            {
            Category.CreateSystem("Salary", "💰", "#4CAF50", "Salary"),
            Category.CreateSystem("Food", "🍔", "#FF6B6B", "Food"),
            Category.CreateSystem("Transport", "🚗", "#4ECDC4", "Transport")
        };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task Handle_EmptyMonth_ReturnsZeroTotals()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var handler = new GetMonthlyBudgetSummaryHandler(context);

            var query = new GetMonthlyBudgetSummaryQuery
            {
                UserId = 1,
                MonthYear = "2026-01"
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalIncome.Should().Be(0);
            result.TotalExpenses.Should().Be(0);
            result.Net.Should().Be(0);
            result.SavingsRate.Should().Be(0);
            result.TransactionCount.Should().Be(0);
            result.CategoryBreakdown.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_OnlyIncome_CalculatesCorrectly()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var handler = new GetMonthlyBudgetSummaryHandler(context);

            var user = User.Create("test", "test@test.com", "hash");
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var income1 = Transaction.Create(
                userId: user.Id,
                categoryId: 1,
                amount: 5000,
                type: TransactionType.Income,
                description: "Salary",
                date: new DateTime(2026, 1, 15)
            );

            var income2 = Transaction.Create(
                userId: user.Id,
                categoryId: 2,
                amount: 500,
                type: TransactionType.Income,
                description: "Freelance",
                date: new DateTime(2026, 1, 20)
            );

            context.Transactions.AddRange(income1, income2);
            await context.SaveChangesAsync(CancellationToken.None);

            var query = new GetMonthlyBudgetSummaryQuery
            {
                UserId = user.Id,
                MonthYear = "2026-01"
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.TotalIncome.Should().Be(5500);
            result.TotalExpenses.Should().Be(0);
            result.Net.Should().Be(5500);
            result.SavingsRate.Should().Be(100);
            result.TransactionCount.Should().Be(2);
            result.CategoryBreakdown.Should().HaveCount(2);
        }
        [Fact]
        public async Task Handle_MixedTransactions_GroupsByCategory()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var handler = new GetMonthlyBudgetSummaryHandler(context);

            var user = User.Create("test", "test@test.com", "hash");
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var transactions = new[]
            {
                Transaction.Create(user.Id, 1, 5000, TransactionType.Income, "Salary", new DateTime(2026, 1, 1)),
                Transaction.Create(user.Id, 2, 50, TransactionType.Expense, "Grocery", new DateTime(2026, 1, 5)),
                Transaction.Create(user.Id, 2, 30, TransactionType.Expense, "Restaurant", new DateTime(2026, 1, 10)),
                Transaction.Create(user.Id, 3, 45, TransactionType.Expense, "Gas", new DateTime(2026, 1, 15))
            };

            context.Transactions.AddRange(transactions);
            await context.SaveChangesAsync(CancellationToken.None);

            var query = new GetMonthlyBudgetSummaryQuery
            {
                UserId = user.Id,
                MonthYear = "2026-01"
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.TotalIncome.Should().Be(5000);
            result.TotalExpenses.Should().Be(125);
            result.Net.Should().Be(4875);
            result.SavingsRate.Should().BeApproximately(97.5m, 0.1m);
            result.TransactionCount.Should().Be(4);
            result.CategoryBreakdown.Should().HaveCount(3);

            var foodCategory = result.CategoryBreakdown.First(c => c.CategoryId == 2);
            foodCategory.Expenses.Should().Be(80);
            foodCategory.TransactionCount.Should().Be(2);
            foodCategory.Net.Should().Be(-80);
        }
        [Fact]
        public async Task Handle_WithBudgetLimits_CalculatesPercentages()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var handler = new GetMonthlyBudgetSummaryHandler(context);

            var user = User.Create("test", "test@test.com", "hash");
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var expense = Transaction.Create(
                userId: user.Id,
                categoryId: 2,
                amount: 150,
                type: TransactionType.Expense,
                description: "Groceries",
                date: new DateTime(2026, 1, 5)
            );
            context.Transactions.Add(expense);

            var budgetLimit = BudgetLimit.Create(user.Id, 2, "2026-01",200);
            context.BudgetLimits.Add(budgetLimit);

            await context.SaveChangesAsync(CancellationToken.None);

            var query = new GetMonthlyBudgetSummaryQuery
            {
                UserId = user.Id,
                MonthYear = "2026-01"
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var foodCategory = result.CategoryBreakdown.First();
            foodCategory.BudgetLimit.Should().Be(200);
            foodCategory.PercentageOfLimit.Should().Be(75);
            foodCategory.IsOverBudget.Should().BeFalse();
        }
        [Fact]
        public async Task Handle_OverBudget_SetsIsOverBudgetTrue()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var handler = new GetMonthlyBudgetSummaryHandler(context);

            var user = User.Create("test", "test@test.com", "hash");
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var expense = Transaction.Create(
                userId: user.Id,
                categoryId: 2,
                amount: 250,
                type: TransactionType.Expense,
                description: "Expensive dinner",
                date: new DateTime(2026, 1, 5)
            );
            context.Transactions.Add(expense);

            var budgetLimit = BudgetLimit.Create(user.Id, 2, "2026-01", 200);
            context.BudgetLimits.Add(budgetLimit);

            await context.SaveChangesAsync(CancellationToken.None);

            var query = new GetMonthlyBudgetSummaryQuery { UserId = user.Id, MonthYear = "2026-01" };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var foodCategory = result.CategoryBreakdown.First();
            foodCategory.Expenses.Should().Be(250);
            foodCategory.BudgetLimit.Should().Be(200);
            foodCategory.PercentageOfLimit.Should().Be(125);
            foodCategory.IsOverBudget.Should().BeTrue();
        }


    }
}
