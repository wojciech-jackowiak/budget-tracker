using BudgetTracker.Application.Auth.Commands.Login;
using BudgetTracker.Application.Auth.Commands.RefreshToken;
using BudgetTracker.Application.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace BudgetTracker.API.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="command">Registration details (username, email, password)</param>
        /// <returns>Auth tokens (access + refresh)</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
             var result = await _mediator.Send(command);
             return CreatedAtAction(nameof(Register), result);
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        /// <param name="command">Login credentials (email, password)</param>
        /// <returns>Auth tokens (access + refresh)</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
             var result = await _mediator.Send(command);
             return Ok(result);
        }
        /// <summary>
        /// Refresh access token using refresh token
        /// </summary>
        /// <param name="command">Refresh token</param>
        /// <returns>New auth tokens</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}