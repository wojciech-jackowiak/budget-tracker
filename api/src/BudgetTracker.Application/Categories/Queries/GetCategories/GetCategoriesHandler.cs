using BudgetTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Application.Categories.Queries.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IBudgetTrackerDbContext _context;

        public GetCategoriesHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Categories.AsQueryable();

            if (request.UserId.HasValue)
            {
                // Systemowe + custom użytkownika
                query = query.Where(c => c.IsSystemDefault || c.UserId == request.UserId.Value);
            }
            else
            {
                // Tylko systemowe
                query = query.Where(c => c.IsSystemDefault);
            }

            var categories = await query
                .OrderBy(c => c.IsSystemDefault ? 0 : 1) // systemowe pierwsze
                .ThenBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IconName = c.IconName,
                    ColorCode = c.ColorCode,
                    IsSystemDefault = c.IsSystemDefault
                })
                .ToListAsync(cancellationToken);

            return categories;
        }
    }
}
