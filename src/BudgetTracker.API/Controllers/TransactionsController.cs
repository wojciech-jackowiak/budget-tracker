using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using BudgetTracker.Application.Expenses.Commands.CreateIncome;
using BudgetTracker.Application.Transactions.Queries.GetTransactions;
using BudgetTracker.Application.Transactions.Queries.GetTransactionsById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BudgetTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }

            return userId;
        }
        /// <summary>
        /// Creates a new expense transaction
        /// </summary>
        /// <param name="command">Expense details</param>
        /// <returns>ID of created transaction</returns>
        [HttpPost("expenses")]
        public async Task<IActionResult> CreateExpense(CreateExpenseCommand command)
        {
            var userId = GetCurrentUserId();
            var secureCommand = command with { UserId = userId };

            var id = await _mediator.Send(secureCommand);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Creates a new income transaction
        /// </summary>
        /// <param name="command">Income details</param>
        /// <returns>ID of created transaction</returns>
        [HttpPost("income")]
        public async Task<IActionResult> CreateIncome(CreateIncomeCommand command)
        {
            var userId = GetCurrentUserId();
            var secureCommand = command with { UserId = userId };

            var id = await _mediator.Send(secureCommand);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        [HttpGet]
        public async Task<IActionResult> GetTransactions(                                                                 
                                                        [FromQuery] string? monthYear,    
                                                        [FromQuery] int? categoryId,      
                                                        [FromQuery] string? type          
                                                    )
        {
            var userId = GetCurrentUserId();
            var query = new GetTransactionsQuery
            {
                UserId = userId,
                MonthYear = monthYear,
                CategoryId = categoryId,
                Type = type
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a transaction by ID
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns>Transaction details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTransactionByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            var userId = GetCurrentUserId();
            if (result.UserId != userId)
            {
                return Forbid(); 
            }

            return Ok(result);
        }
    }
}
