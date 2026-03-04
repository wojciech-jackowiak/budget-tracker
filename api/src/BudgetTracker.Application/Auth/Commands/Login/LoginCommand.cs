using BudgetTracker.Application.Auth.Common.DTOs;
using MediatR;
namespace BudgetTracker.Application.Auth.Commands.Login
{
    public record LoginCommand : IRequest<AuthResponseDto>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}