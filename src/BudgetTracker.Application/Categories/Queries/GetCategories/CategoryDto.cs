using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Categories.Queries.GetCategories
{
    public record CategoryDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string IconName { get; init; } = string.Empty;
        public string ColorCode { get; init; } = string.Empty;
        public bool IsSystemDefault { get; init; }
        public bool IsCustom => !IsSystemDefault;
    }
}
