using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    IsSystemDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetLimits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    MonthYear = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    LimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLimits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLimits_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetLimits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecurringTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastProcessedMonth = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringTransactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthYear = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RecurringTransactionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_RecurringTransactions_RecurringTransactionId",
                        column: x => x.RecurringTransactionId,
                        principalTable: "RecurringTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorCode", "CreatedAt", "Description", "IconName", "IsSystemDefault", "Name", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "#4CAF50", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Monthly salary and wages", "💰", true, "Salary", null, null },
                    { 2, "#2196F3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Freelance income and side projects", "💼", true, "Freelance", null, null },
                    { 3, "#9C27B0", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Investment returns and dividends", "📈", true, "Investments", null, null },
                    { 4, "#FF6B6B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Groceries, restaurants, and food delivery", "🍔", true, "Food & Dining", null, null },
                    { 5, "#4ECDC4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fuel, public transport, and car maintenance", "🚗", true, "Transportation", null, null },
                    { 6, "#95E1D3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Movies, games, subscriptions, and hobbies", "🎬", true, "Entertainment", null, null },
                    { 7, "#F3A683", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Electricity, water, internet, and phone bills", "💡", true, "Utilities", null, null },
                    { 8, "#786FA6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medical expenses, insurance, and pharmacy", "🏥", true, "Healthcare", null, null },
                    { 9, "#F8B500", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Clothing, electronics, and general shopping", "🛍️", true, "Shopping", null, null },
                    { 10, "#3F51B5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Courses, books, and learning materials", "📚", true, "Education", null, null },
                    { 11, "#E91E63", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rent, mortgage, and home maintenance", "🏠", true, "Housing", null, null },
                    { 12, "#00BCD4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Personal savings and emergency fund", "🐷", true, "Savings", null, null },
                    { 13, "#FF9800", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Presents, charity, and donations", "🎁", true, "Gifts & Donations", null, null },
                    { 14, "#009688", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vacations, trips, and accommodation", "✈️", true, "Travel", null, null },
                    { 15, "#E91E63", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Haircuts, cosmetics, and wellness", "💅", true, "Personal Care", null, null },
                    { 16, "#607D8B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Life, health, and property insurance", "🛡️", true, "Insurance", null, null },
                    { 17, "#F44336", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Loan payments and credit card bills", "💳", true, "Debt Payment", null, null },
                    { 18, "#8BC34A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pet food, vet, and supplies", "🐕", true, "Pets", null, null },
                    { 19, "#FF5722", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gym, equipment, and sports activities", "⚽", true, "Sports & Fitness", null, null },
                    { 20, "#9E9E9E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Miscellaneous expenses", "📌", true, "Other", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLimits_CategoryId",
                table: "BudgetLimits",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLimits_UserId_CategoryId_MonthYear",
                table: "BudgetLimits",
                columns: new[] { "UserId", "CategoryId", "MonthYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsSystemDefault_Name",
                table: "Categories",
                columns: new[] { "IsSystemDefault", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactions_CategoryId",
                table: "RecurringTransactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactions_UserId_IsActive",
                table: "RecurringTransactions",
                columns: new[] { "UserId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_ExpiresAt",
                table: "RefreshTokens",
                columns: new[] { "UserId", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Date",
                table: "Transactions",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecurringTransactionId",
                table: "Transactions",
                column: "RecurringTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId_MonthYear",
                table: "Transactions",
                columns: new[] { "UserId", "MonthYear" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetLimits");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "RecurringTransactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
