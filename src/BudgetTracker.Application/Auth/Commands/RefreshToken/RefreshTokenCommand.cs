using BudgetTracker.Application.Auth.Common.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<AuthResponseDto>
    {
        public required string RefreshToken { get; init; }

    }
}