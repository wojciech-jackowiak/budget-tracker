using BudgetTracker.Application.Categories.Queries.GetCategories;
using BudgetTracker.Application.Transactions.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Gets all categories (system categories + user custom categories if userId provided)
        /// </summary>
        /// <param name="userId">User ID (optional, if provided returns system + user categories)</param>
        /// <returns>List of categories</returns>
        [HttpGet]
        public async Task<IActionResult> GetCategories(int? userId = null)
        {
            var query = new GetCategoriesQuery
            {
                UserId = userId,
            };
            var result = await _mediator.Send(query);
            return Ok(result);

        }        
    }
}
