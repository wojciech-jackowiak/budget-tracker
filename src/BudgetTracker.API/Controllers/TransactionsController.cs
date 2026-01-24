using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using BudgetTracker.Application.Expenses.Commands.CreateIncome;
using BudgetTracker.Application.Transactions.Commands.DeleteTransaction;
using BudgetTracker.Application.Transactions.Commands.UpdateTransaction;
using BudgetTracker.Application.Transactions.Queries.GetTransactions;
using BudgetTracker.Application.Transactions.Queries.GetTransactionsById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BudgetTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransactionsController> _logger;
        public TransactionsController(IMediator mediator, ILogger<TransactionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            _logger.LogInformation("=== GetCurrentUserId DEBUG ===");
            _logger.LogInformation($"User.Identity.IsAuthenticated: {User.Identity?.IsAuthenticated}");
            _logger.LogInformation($"User.Identity.Name: {User.Identity?.Name}");
            _logger.LogInformation($"User.Identity.AuthenticationType: {User.Identity?.AuthenticationType}");
            _logger.LogInformation($"Claims count: {User.Claims.Count()}");

            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"  Claim Type: '{claim.Type}', Value: '{claim.Value}'");
            }

            // Spróbuj różne claim types
            var subClaim = User.FindFirst("sub")?.Value;
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var longNameIdentifierClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            _logger.LogInformation($"sub claim: '{subClaim}'");
            _logger.LogInformation($"NameIdentifier claim: '{nameIdentifierClaim}'");
            _logger.LogInformation($"Long NameIdentifier claim: '{longNameIdentifierClaim}'");

            var userIdClaim = subClaim ?? nameIdentifierClaim ?? longNameIdentifierClaim;

            _logger.LogInformation($"Final userIdClaim: '{userIdClaim}'");

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogError("User ID not found in token or invalid format");
                throw new UnauthorizedAccessException("User ID not found in token");
            }

            _logger.LogInformation($"UserId extracted: {userId}");
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
        public async Task<IActionResult> GetTransactions([FromQuery] string? monthYear,[FromQuery] int? categoryId,[FromQuery] string? type)
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
        /// <summary>
        /// Updates an existing transaction
        /// </summary>
        /// <param name="command">Updated transaction details</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateTransactionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Route ID and command ID must match");
            }
            var userId = GetCurrentUserId();            

            var secureCommand = command with { UserId = userId };

            await _mediator.Send(secureCommand);

            return NoContent();
        }
        /// <summary>
        /// Deletes a transaction
        /// </summary>
        /// <param name="id">Transaction ID to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();

            var command = new DeleteTransactionCommand
            {
                Id = id,
                UserId = userId
            };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
