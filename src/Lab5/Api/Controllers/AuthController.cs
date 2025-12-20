using Core.Application.Abstractions.Services;
using Core.Domain.Results;
using Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public AuthController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public record LoginUserRequest(string AccountNumber, string Pin);

    public record LoginAdminRequest(string Password);

    public record LoginResponse(Guid SessionId);

    [HttpPost("user")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        SessionResult result = await _sessionService.LoginUserAsync(
            new AccountNumber(request.AccountNumber),
            request.Pin,
            cancellationToken);

        return result switch
        {
            SessionResult.Success success => Ok(new LoginResponse(success.AtmSession.SessionId)),
            SessionResult.Failure => Unauthorized(new { message = "Wrong password or PIN" }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    [HttpPost("admin")]
    public async Task<IActionResult> LoginAdmin(
        [FromBody] LoginAdminRequest request,
        CancellationToken cancellationToken)
    {
        SessionResult result = await _sessionService.LoginAdminAsync(request.Password, cancellationToken);

        return result switch
        {
            SessionResult.Success success => Ok(new LoginResponse(success.AtmSession.SessionId)),
            SessionResult.Failure => Unauthorized(new { message = "Wrong system password" }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }
}