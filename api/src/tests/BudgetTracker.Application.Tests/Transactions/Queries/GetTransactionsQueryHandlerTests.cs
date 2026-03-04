using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Application.Transactions.Queries.GetTransactions;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace BudgetTracker.Application.Tests.Transactions.Queries
{
    public class GetTransactionsQueryHandlerTests
    {
        private readonly Mock<IBudgetTrackerDbContext> _contextMock;
        private readonly GetTransactionsHandler _handler;

        public GetTransactionsQueryHandlerTests()
        {
            _contextMock = new Mock<IBudgetTrackerDbContext>();
            _handler = new GetTransactionsHandler(_contextMock.Object);
        }

        private void SetupTransactions(List<Transaction> transactions, List<Category> categories)
        {
            var mockCategoriesSet = categories.BuildMockDbSet();
            _contextMock
                .Setup(x => x.Categories)
                .Returns(mockCategoriesSet.Object);

            var mockTransactionsSet = transactions.BuildMockDbSet();
            _contextMock
                .Setup(x => x.Transactions)
                .Returns(mockTransactionsSet.Object);
        }

        private Category CreateTestCategory(int id, string name = "Test Category")
        {
            var category = Category.CreateSystem(name, "📝", "#999999");
            typeof(Category).GetProperty("Id")!.SetValue(category, id);
            return category;
        }

        private Transaction CreateTestTransaction(
            int id,
            int userId,
            decimal amount,
            TransactionType type,
            string description,
            Category category,
            DateTime? date = null)
        {
            var transaction = Transaction.Create(
                userId: userId,
                categoryId: category.Id,
                amount: amount,
                type: type,
                description: description,
                date: date ?? DateTime.UtcNow
            );

            // Set Id i Category przez reflection
            typeof(Transaction).GetProperty("Id")!.SetValue(transaction, id);
            typeof(Transaction).GetProperty("Category")!.SetValue(transaction, category);

            return transaction;
        }

        [Fact]
        public async Task Handle_WithNoTransactions_ShouldReturnEmptyList()
        {
            // Arrange
            SetupTransactions(new List<Transaction>(), new List<Category>());
            var query = new GetTransactionsQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WithExistingExpenses_ShouldReturnExpensesList()
        {
            // Arrange
            var category = CreateTestCategory(1, "Food");
            var categories = new List<Category> { category };

            var transactions = new List<Transaction>
        {
            CreateTestTransaction(1, 1, 50m, TransactionType.Expense, "Lunch", category),
            CreateTestTransaction(2, 1, 100m, TransactionType.Expense, "Dinner", category)
        };

            SetupTransactions(transactions, categories);
            var query = new GetTransactionsQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.All(t => t.Type == "Expense").Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnBothIncomesAndExpenses_WhenNoTypeFilter()
        {
            // Arrange
            var category = CreateTestCategory(1, "Mixed");
            var categories = new List<Category> { category };

            var transactions = new List<Transaction>
        {
            CreateTestTransaction(1, 1, 50m, TransactionType.Expense, "Lunch", category),
            CreateTestTransaction(2, 1, 1500m, TransactionType.Income, "Salary", category)
        };

            SetupTransactions(transactions, categories);
            var query = new GetTransactionsQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(t => t.Type == "Expense");
            result.Should().Contain(t => t.Type == "Income");
        }

        [Fact]
        public async Task Handle_WithTypeFilter_ShouldReturnOnlyExpenses()
        {
            // Arrange
            var category = CreateTestCategory(1);
            var categories = new List<Category> { category };

            var transactions = new List<Transaction>
        {
            CreateTestTransaction(1, 1, 50m, TransactionType.Expense, "Lunch", category),
            CreateTestTransaction(2, 1, 1500m, TransactionType.Income, "Salary", category)
        };

            SetupTransactions(transactions, categories);
            var query = new GetTransactionsQuery
            {
                UserId = 1,
                Type = "Expense"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result[0].Type.Should().Be("Expense");
        }

        [Fact]
        public async Task Handle_ShouldReturnOnlyUserTransactions_NotOtherUsers()
        {
            // Arrange
            var category = CreateTestCategory(1);
            var categories = new List<Category> { category };

            var transactions = new List<Transaction>
        {
            CreateTestTransaction(1, 1, 3m, TransactionType.Expense, "Coffee", category),
            CreateTestTransaction(2, 2, 77m, TransactionType.Expense, "Gas station", category)
        };

            SetupTransactions(transactions, categories);
            var query = new GetTransactionsQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result[0].UserId.Should().Be(1);
        }

        [Fact]
        public async Task Handle_WithMonthYearFilter_ShouldReturnOnlyMatchingMonth()
        {
            // Arrange
            var category = CreateTestCategory(1);
            var categories = new List<Category> { category };

            var transactions = new List<Transaction>
        {
            CreateTestTransaction(1, 1, 50m, TransactionType.Expense, "Jan expense", category, new DateTime(2026, 1, 15)),
            CreateTestTransaction(2, 1, 100m, TransactionType.Expense, "Feb expense", category, new DateTime(2026, 2, 15))
        };

            SetupTransactions(transactions, categories);
            var query = new GetTransactionsQuery
            {
                UserId = 1,
                MonthYear = "2026-01"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result[0].MonthYear.Should().Be("2026-01");
        }

        [Fact]
        public async Task Handle_WithCategoryFilter_ShouldReturnOnlyMatchingCategory()
        {
            // Arrange
            var foodCategory = CreateTestCategory(1, "Food");
            var transportCategory = CreateTestCategory(2, "Transport");
            var categories = new List<Category> { foodCategory, transportCategory };

            var transactions = new List<Transaction>
        {
            CreateTestTransaction(1, 1, 50m, TransactionType.Expense, "Lunch", foodCategory),
            CreateTestTransaction(2, 1, 30m, TransactionType.Expense, "Bus", transportCategory)
        };

            SetupTransactions(transactions, categories);
            var query = new GetTransactionsQuery
            {
                UserId = 1,
                CategoryId = 1
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result[0].CategoryId.Should().Be(1);
            result[0].CategoryName.Should().Be("Food");
        }

        [Fact]
        public async Task Handle_ShouldIncludeCategoryInformation()
        {
            // Arrange
            var category = CreateTestCategory(1, "Test Category");
            var categories = new List<Category> { category };

            var transactions = new List<Transaction>
        {
            CreateTestTransaction(1, 1, 50m, TransactionType.Expense, "Test", category)
        };

            SetupTransactions(transactions, categories);
            var query = new GetTransactionsQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result[0].CategoryId.Should().Be(1);
            result[0].CategoryName.Should().Be("Test Category");
            result[0].CategoryIcon.Should().Be("📝");
            result[0].CategoryColor.Should().Be("#999999");
        }

        [Fact]
        public async Task Handle_ShouldCalculateSignedAmountCorrectly()
        {
            // Arrange
            var category = CreateTestCategory(1);
            var categories = new List<Category> { category };

            var transactions = new List<Transaction>
        {
            CreateTestTransaction(1, 1, 100m, TransactionType.Income, "Salary", category),
            CreateTestTransaction(2, 1, 50m, TransactionType.Expense, "Lunch", category)
        };

            SetupTransactions(transactions, categories);
            var query = new GetTransactionsQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var income = result.First(t => t.Type == "Income");
            var expense = result.First(t => t.Type == "Expense");

            income.SignedAmount.Should().Be(100m);   //Positive
            expense.SignedAmount.Should().Be(-50m);  //Negative
        }
    }
}
