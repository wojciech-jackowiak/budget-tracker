using BudgetTracker.Application.Budget.Queries.GetMonthlyBudgetSummary;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BudgetController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Gets monthly budget summary with category breakdown
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="monthYear">Month in YYYY-MM format (e.g., 2026-01)</param>
        /// <returns>Budget summary with income, expenses, and category breakdown</returns>
        [HttpGet("summary")]
        public async Task<IActionResult> GetMonthlySummary(
            [FromQuery] int userId,
            [FromQuery] string monthYear)
        {
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