using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using BudgetTracker.Application.Expenses.Commands.CreateIncome;
using BudgetTracker.Application.Transactions.Queries.GetTransactions;
using BudgetTracker.Application.Transactions.Queries.GetTransactionsById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Creates a new expense transaction
        /// </summary>
        /// <param name="command">Expense details</param>
        /// <returns>ID of created transaction</returns>
        [HttpPost("expenses")]
        public async Task<IActionResult> CreateExpense(CreateExpenseCommand command)
        {
            var id = await _mediator.Send(command);
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
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        [HttpGet]
        public async Task<IActionResult> GetTransactions(
                                                        [FromQuery] int userId,           
                                                        [FromQuery] string? monthYear,    
                                                        [FromQuery] int? categoryId,      
                                                        [FromQuery] string? type          
                                                    )
        {
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
            
            return Ok(result);
        }
    }
}
