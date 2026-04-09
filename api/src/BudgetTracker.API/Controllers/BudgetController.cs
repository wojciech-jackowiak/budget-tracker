using BudgetTracker.Application.Budget.Queries.GetMonthlyBudgetSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BudgetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("sub")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User ID not found in token");

            return userId;
        }

        /// <summary>
        /// Gets monthly budget summary with category breakdown
        /// </summary>
        /// <param name="monthYear">Month in YYYY-MM format (e.g., 2026-01)</param>
        /// <returns>Budget summary with income, expenses, and category breakdown</returns>
        [HttpGet("summary")]
        public async Task<IActionResult> GetMonthlySummary([FromQuery] string monthYear)
        {
            var userId = GetCurrentUserId();
            var query = new GetMonthlyBudgetSummaryQuery
            {
                UserId = userId,
                MonthYear = monthYear
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}