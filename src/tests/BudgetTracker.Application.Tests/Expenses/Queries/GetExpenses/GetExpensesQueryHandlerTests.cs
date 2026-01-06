using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Application.Expenses.Queries.GetExpenses;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
namespace BudgetTracker.Application.Tests.Expenses.Queries.GetExpenses
{
    public class GetExpensesQueryHandlerTests
    {
        private readonly Mock<IBudgetTrackerDbContext> _contextMock;
        private readonly GetExpensesQueryHandler _handler;
        private List<Transaction> _transactions;
        public GetExpensesQueryHandlerTests()
        {
            _contextMock = new Mock<IBudgetTrackerDbContext>();
            _handler = new GetExpensesQueryHandler(_contextMock.Object);
        }
        private void SetupTransactions(List<Transaction> transactions)
        {
            var mockDbSet = transactions.BuildMockDbSet<Transaction>();

            _contextMock
                .Setup(x => x.Transactions)
                .Returns(mockDbSet.Object);
        }

        [Fact]
        public async Task Handle_WithNoExpenses_ShouldReturnEmptyList()
        {
            // Arrange
            SetupTransactions(new List<Transaction>());

            var query = new GetExpensesQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WithExistingExpenses_ShouldReturnExpensesList()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    UserId = 1,
                    Amount = 50m,
                    Type = TransactionType.Expense,
                    Date = DateTime.UtcNow,
                    Description = "Lunch"
                },
                new Transaction
                {
                    Id = 2,
                    UserId = 1,
                    Amount = 100m,
                    Type = TransactionType.Expense,
                    Date = DateTime.UtcNow,
                    Description = "Dinner"
                }
            };

            SetupTransactions(transactions);

            var query = new GetExpensesQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_ShouldReturnOnlyExpenses_NotIncomes()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    UserId = 1,
                    Amount = 50m,
                    Type = TransactionType.Expense,
                    Date = DateTime.UtcNow,
                    Description = "Lunch"
                },
                new Transaction
                {
                    Id = 2,
                    UserId = 1,
                    Amount = 1500,
                    Type = TransactionType.Income,
                    Date = DateTime.UtcNow,
                    Description = "Salary"
                },
            };
            SetupTransactions(transactions);
            var query = new GetExpensesQuery { UserId = 1 };
            var result = await _handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(1);
        }
        [Fact]
        public async Task Handle_ShouldReturnOnlyUserExpenses_NotOtherUsers()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    UserId = 1,
                    Amount = 3,
                    Type = TransactionType.Expense,
                    Date = DateTime.UtcNow,
                    Description = "Coffee"
                },
                new Transaction
                {
                    Id = 2,
                    UserId = 2,
                    Amount = 77,
                    Type = TransactionType.Expense,
                    Date = DateTime.UtcNow,
                    Description = "Gas station"
                },
            };
            SetupTransactions(transactions);
            var query = new GetExpensesQuery { UserId = 1 };
            var result = await _handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(1);
        }
    }
}
