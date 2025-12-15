using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Authentication;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.ObjectOrientedProgramming.Lab5.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    private readonly ICurrentSessionService _currentSession;
    private readonly ISessionService _sessionService;

    public UserController(
        IAccountService accountService,
        ITransactionService transactionService,
        ICurrentSessionService currentSession,
        ISessionService sessionService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
        _currentSession = currentSession;
        _sessionService = sessionService;
    }

    public record MoneyRequest(decimal Amount);

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        UnauthorizedObjectResult? check = RequireUserSession();
        if (check is not null) return check;

        AtmSession? session = _currentSession.CurrentSession;
        if (session is null)
            return Unauthorized(new { message = "User session is required" });

        BalanceResult result = await _accountService.GetBalanceAsync(session.AccountNumber);

        return result switch
        {
            BalanceResult.Success success => Ok(new { balance = success.Balance.Amount }),
            BalanceResult.Failure failure => BadRequest(new { failure.Code, failure.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] MoneyRequest request)
    {
        UnauthorizedObjectResult? check = RequireUserSession();
        if (check is not null) return check;

        AtmSession? session = _currentSession.CurrentSession;
        if (session is null) return Unauthorized();

        TransactionResult result = await _accountService.DepositAsync(session.AccountNumber, new Money(request.Amount));

        return result switch
        {
            TransactionResult.Success => Ok(new { message = "Money deposited successfully" }),
            TransactionResult.Failure failure => BadRequest(new { failure.Code, failure.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] MoneyRequest request)
    {
        UnauthorizedObjectResult? check = RequireUserSession();
        if (check is not null) return check;

        AtmSession? session = _currentSession.CurrentSession;
        if (session is null) return Unauthorized();

        TransactionResult result =
            await _accountService.WithdrawAsync(session.AccountNumber, new Money(request.Amount));

        return result switch
        {
            TransactionResult.Success => Ok(new { message = "Money withdrawn successfully" }),
            TransactionResult.Failure failure => BadRequest(new { failure.Code, failure.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        UnauthorizedObjectResult? check = RequireUserSession();
        if (check is not null) return check;

        AtmSession? session = _currentSession.CurrentSession;
        if (session is null) return Unauthorized();

        IReadOnlyList<Transaction> history = await _transactionService.GetHistoryAsync(session.AccountId);

        return Ok(history.Select(t => new
        {
            t.Id,
            t.Type,
            t.Amount.Amount,
            NewBalance = t.NewBalance.Amount,
            t.CreatedAt,
        }));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        AtmSession? session = _currentSession.CurrentSession;
        if (session is null)
            return BadRequest(new { message = "No active session" });

        await _sessionService.LogoutAsync(session.SessionId);

        return Ok(new { message = "Logged out successfully" });
    }

    private UnauthorizedObjectResult? RequireUserSession()
    {
        AtmSession? session = _currentSession.CurrentSession;
        if (session?.Account is null)
            return Unauthorized(new { message = "User session is required" });

        return null;
    }
}