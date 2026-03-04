using BudgetTracker.Application.RecurringTransactions.Commands.Create;
using BudgetTracker.Application.RecurringTransactions.Commands.Delete;
using BudgetTracker.Application.RecurringTransactions.Commands.Pause;
using BudgetTracker.Application.RecurringTransactions.Commands.Resume;
using BudgetTracker.Application.RecurringTransactions.Commands.Update;
using BudgetTracker.Application.RecurringTransactions.Queries.GetRecurringTransactions;
using BudgetTracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringTransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RecurringTransactionsController> _logger;

        public RecurringTransactionsController(
            IMediator mediator,
            ILogger<RecurringTransactionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }

            return userId;
        }
        private TransactionType? ParseEnum(string typeString)
        {
            TransactionType? typeEnum = null;
            if (!string.IsNullOrEmpty(typeString))
            {
                if (Enum.TryParse<TransactionType>(typeString, ignoreCase: true, out var parsedType))
                {
                    typeEnum = parsedType;
                }
                else
                {
                    return null;

                }
            }
            return typeEnum;
        }
        /// <summary>
        /// Creates a new recurring transaction
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateRecurringTransactionCommand command)
        {
            var userId = GetCurrentUserId();
            var secureCommand = command with { UserId = userId };

            var id = await _mediator.Send(secureCommand);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        /// <summary>
        /// Gets all recurring transactions for current user with optional filters
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool? isActive = null,
            [FromQuery] string? type = null)
        {
            var userId = GetCurrentUserId();
            var typeEnum = ParseEnum(type);
            if(!typeEnum.HasValue) return BadRequest($"Invalid transaction type: {type}. Valid values: Income, Expense");

            var query = new GetRecurringTransactionsQuery
            {
                UserId = userId,
                IsActive = isActive,
                Type = typeEnum
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a single recurring transaction by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetCurrentUserId();

            var query = new GetRecurringTransactionsQuery
            {
                UserId = userId
            };

            var result = await _mediator.Send(query);
            var recurring = result.FirstOrDefault(r => r.Id == id);

            if (recurring == null)
                return NotFound();

            if (recurring.UserId != userId)
                return Forbid();

            return Ok(recurring);
        }

        /// <summary>
        /// Updates an existing recurring transaction
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateRecurringTransactionCommand command)
        {
            var userId = GetCurrentUserId();

            if (id != command.Id)
            {
                return BadRequest("Route ID and command ID must match");
            }

            var secureCommand = command with { UserId = userId };

            await _mediator.Send(secureCommand);

            return NoContent();
        }

        /// <summary>
        /// Deletes a recurring transaction (stops future generations)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();

            var command = new DeleteRecurringTransactionCommand
            {
                Id = id,
                UserId = userId
            };

            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Pauses a recurring transaction (sets IsActive = false)
        /// </summary>
        [HttpPost("{id}/pause")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Pause(int id)
        {
            var userId = GetCurrentUserId();

            var command = new PauseRecurringTransactionCommand
            {
                Id = id,
                UserId = userId
            };

            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Resumes a paused recurring transaction (sets IsActive = true)
        /// </summary>
        [HttpPost("{id}/resume")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Resume(int id)
        {
            var userId = GetCurrentUserId();

            var command = new ResumeRecurringTransactionCommand
            {
                Id = id,
                UserId = userId
            };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
