using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using BudgetTracker.Application.Expenses.Commands.CreateIncome;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using MockQueryable.Moq;

namespace BudgetTracker.Application.Tests.Incomes.Commands
{
    public class CreateIncomeCommandTests
    {
        private readonly Mock<IBudgetTrackerDbContext> _contextMock;
        private readonly CreateIncomeHandler _handler;
        private readonly List<Transaction> _transactions;
        private readonly List<Category> _categories;
        private int _idCounter = 1;
        public CreateIncomeCommandTests()
        {
            _contextMock = new Mock<IBudgetTrackerDbContext>();
            _transactions = new List<Transaction>();
            _categories = new List<Category>();

            SetupCategoriesDbSet();
            SetupTransactionsDbSet();

            _handler = new CreateIncomeHandler(_contextMock.Object);
        }
        private void SetupCategoriesDbSet()
        {
            var testCategory = Category.CreateSystem("Test Category", "📝", "#999999");
            typeof(Category).GetProperty("Id")!.SetValue(testCategory, 1);
            _categories.Add(testCategory);

            var mockCategoriesSet = _categories.BuildMockDbSet<Category>();

            _contextMock
                .Setup(x => x.Categories)
                .Returns(mockCategoriesSet.Object);
        }
        private void SetupTransactionsDbSet()
        {
            var mockSet = new Mock<DbSet<Transaction>>();

            mockSet
                .Setup(m => m.Add(It.IsAny<Transaction>()))
                .Callback<Transaction>(t =>
                {
                    typeof(Transaction).GetProperty("Id")!.SetValue(t, _idCounter++);
                    _transactions.Add(t);
                });

            _contextMock
                .Setup(x => x.Transactions)
                .Returns(mockSet.Object);

            _contextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldReturnNewTransactionId()
        {
            // Arrange
            var command = new CreateIncomeCommand
            {
                Amount = 1000m,
                Description = "Monthly salary",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateTransactionWithCorrectData()
        {
            // Arrange
            var command = new CreateIncomeCommand
            {
                Amount = 2500m,
                Description = "Freelance payment",
                Date = new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc),
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions.Should().HaveCount(1);

            var created = _transactions[0];
            created.Amount.Should().Be(2500m);
            created.Description.Should().Be("Freelance payment");
            created.CategoryId.Should().Be(1);
            created.UserId.Should().Be(1);
            created.Type.Should().Be(TransactionType.Income);
            created.MonthYear.Should().Be("2026-01");
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldSetTransactionTypeToIncome()
        {
            // Arrange
            var command = new CreateIncomeCommand
            {
                Amount = 250m,
                Description = "Tax return",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions[0].Type.Should().Be(TransactionType.Income);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCallSaveChangesAsync()
        {
            // Arrange
            var command = new CreateIncomeCommand
            {
                Amount = 500m,
                Description = "Bonus",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _contextMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentCategory_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new CreateIncomeCommand
            {
                Amount = 1000m,
                Description = "Salary",
                Date = DateTime.UtcNow,
                CategoryId = 999, 
                UserId = 1
            };

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BudgetTracker.Application.Common.Exceptions.NotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldSetMonthYearAutomatically()
        {
            // Arrange
            var testDate = new DateTime(2026, 3, 20);
            var command = new CreateIncomeCommand
            {
                Amount = 1500m,
                Description = "Salary",
                Date = testDate,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions[0].MonthYear.Should().Be("2026-03");
        }

    }
}
