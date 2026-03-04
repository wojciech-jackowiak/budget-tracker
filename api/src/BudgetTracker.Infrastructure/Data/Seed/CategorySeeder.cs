using BudgetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Infrastructure.Data.Seed
{
    public class CategorySeeder
    {
        public static void SeedCategories(ModelBuilder modelBuilder)
        {
            var categories = new[]
            {
            CreateSystemCategory(1, "Salary", "💰", "#4CAF50", "Monthly salary and wages"),
            CreateSystemCategory(2, "Freelance", "💼", "#2196F3", "Freelance income and side projects"),
            CreateSystemCategory(3, "Investments", "📈", "#9C27B0", "Investment returns and dividends"),
            CreateSystemCategory(4, "Food & Dining", "🍔", "#FF6B6B", "Groceries, restaurants, and food delivery"),
            CreateSystemCategory(5, "Transportation", "🚗", "#4ECDC4", "Fuel, public transport, and car maintenance"),
            CreateSystemCategory(6, "Entertainment", "🎬", "#95E1D3", "Movies, games, subscriptions, and hobbies"),
            CreateSystemCategory(7, "Utilities", "💡", "#F3A683", "Electricity, water, internet, and phone bills"),
            CreateSystemCategory(8, "Healthcare", "🏥", "#786FA6", "Medical expenses, insurance, and pharmacy"),
            CreateSystemCategory(9, "Shopping", "🛍️", "#F8B500", "Clothing, electronics, and general shopping"),
            CreateSystemCategory(10, "Education", "📚", "#3F51B5", "Courses, books, and learning materials"),
            CreateSystemCategory(11, "Housing", "🏠", "#E91E63", "Rent, mortgage, and home maintenance"),
            CreateSystemCategory(12, "Savings", "🐷", "#00BCD4", "Personal savings and emergency fund"),
            CreateSystemCategory(13, "Gifts & Donations", "🎁", "#FF9800", "Presents, charity, and donations"),
            CreateSystemCategory(14, "Travel", "✈️", "#009688", "Vacations, trips, and accommodation"),
            CreateSystemCategory(15, "Personal Care", "💅", "#E91E63", "Haircuts, cosmetics, and wellness"),
            CreateSystemCategory(16, "Insurance", "🛡️", "#607D8B", "Life, health, and property insurance"),
            CreateSystemCategory(17, "Debt Payment", "💳", "#F44336", "Loan payments and credit card bills"),
            CreateSystemCategory(18, "Pets", "🐕", "#8BC34A", "Pet food, vet, and supplies"),
            CreateSystemCategory(19, "Sports & Fitness", "⚽", "#FF5722", "Gym, equipment, and sports activities"),
            CreateSystemCategory(20, "Other", "📌", "#9E9E9E", "Miscellaneous expenses")
        };

            modelBuilder.Entity<Category>().HasData(categories);
        }

        private static object CreateSystemCategory(int id, string name, string icon, string color, string description)
        {
            return new
            {
                Id = id,
                Name = name,
                IconName = icon,
                ColorCode = color,
                Description = description,
                IsSystemDefault = true,
                UserId = (int?)null,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = (DateTime?)null
            };
        }
    }
}
