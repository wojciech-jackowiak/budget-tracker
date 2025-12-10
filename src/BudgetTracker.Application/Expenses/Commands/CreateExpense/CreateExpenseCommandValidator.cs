using FluentValidation;

namespace BudgetTracker.Application.Expenses.Commands.CreateExpense
{
    public  class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
    {
        public CreateExpenseCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(200)
                .WithMessage("Description cannot exceed 200 characters");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required")
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                .WithMessage("Date cannot be in the future");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("Valid user is required");
        }
    }
}
