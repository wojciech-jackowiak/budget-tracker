using BudgetTracker.Application.Auth.Common.DTOs;
using MediatR;

namespace BudgetTracker.Application.Auth.Commands.Register
{
    public record RegisterCommand : IRequest<AuthResponseDto>
    {
        public required string Username { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}