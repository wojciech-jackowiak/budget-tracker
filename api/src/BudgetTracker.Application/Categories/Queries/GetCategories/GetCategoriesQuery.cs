using MediatR;


namespace BudgetTracker.Application.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<List<CategoryDto>>
    {
        public int? UserId { get; init; } // null = tylko systemowe, not null = systemowe + custom użytkownika
    }
}
