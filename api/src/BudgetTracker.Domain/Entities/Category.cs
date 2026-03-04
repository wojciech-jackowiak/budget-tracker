using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string IconName { get; private set; }
        public string ColorCode { get; private set; }
        public bool IsSystemDefault { get; private set; }
        public int? UserId { get; private set; }
        public User? User { get; private set; }

        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private Category()
        {
            Name = string.Empty;
            IconName = string.Empty;
            ColorCode = string.Empty;
        }

        public static Category CreateSystem(string name, string iconName, string colorCode, string? description = null)
        {
            ValidateName(name);
            ValidateColorCode(colorCode);

            return new Category
            {
                Name = name,
                Description = description,
                IconName = iconName,
                ColorCode = colorCode,
                IsSystemDefault = true,
                UserId = null
            };
        }

        public static Category CreateCustom(string name, int userId, string iconName = "📁", string colorCode = "#999999", string? description = null)
        {
            ValidateName(name);
            ValidateColorCode(colorCode);

            return new Category
            {
                Name = name,
                Description = description,
                IconName = iconName,
                ColorCode = colorCode,
                IsSystemDefault = false,
                UserId = userId
            };
        }

        public void Update(string name, string iconName, string colorCode, string? description = null)
        {
            if (IsSystemDefault)
                throw new DomainException("Cannot modify system category");

            ValidateName(name);
            ValidateColorCode(colorCode);

            Name = name;
            IconName = iconName;
            ColorCode = colorCode;
            Description = description;
            MarkAsModified();
        }

        public bool CanBeDeleted()
        {
            return !IsSystemDefault && !_transactions.Any();
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name cannot be empty");

            if (name.Length > 50)
                throw new DomainException("Category name too long (max 50 characters)");
        }

        private static void ValidateColorCode(string colorCode)
        {
            var hexPattern = @"^#[0-9A-Fa-f]{6}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(colorCode, hexPattern))
                throw new DomainException("Invalid color code format (expected #RRGGBB)");
        }
    }
}
