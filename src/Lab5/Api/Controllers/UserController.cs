using Core.Application.Abstractions.Services;
using Core.Domain.Entities;
using Core.Domain.Results;
using Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    private readonly ISessionService _sessionService;

    public UserController(
        IAccountService accountService,
        ITransactionService transactionService,
        ISessionService sessionService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
        _sessionService = sessionService;
    }

    public record MoneyRequest(decimal Amount);

    private AccountNumber CurrentAccountNumber => new(
        User.FindFirst("AccountNumber")?.Value
        ?? throw new InvalidOperationException("AccountNumber claim missing"));

    private Guid CurrentAccountId => Guid.Parse(
        User.FindFirst("AccountId")?.Value
        ?? throw new InvalidOperationException("AccountId claim missing"));

    private Guid CurrentSessionId => Guid.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new InvalidOperationException("SessionId claim missing"));

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance(CancellationToken cancellationToken)
    {
        BalanceResult result = await _accountService.GetBalanceAsync(CurrentAccountNumber, cancellationToken);

        return result switch
        {
            BalanceResult.Success success => Ok(new { balance = success.Balance.Amount }),
            BalanceResult.Failure failure => BadRequest(new { failure.Code, failure.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] MoneyRequest request, CancellationToken cancellationToken)
    {
        TransactionResult result = await _accountService.DepositAsync(
            CurrentAccountNumber,
            new Money(request.Amount),
            cancellationToken);

        return result switch
        {
            TransactionResult.Success => Ok(new { message = "Money deposited successfully" }),
            TransactionResult.Failure failure => BadRequest(new { failure.Code, failure.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] MoneyRequest request, CancellationToken cancellationToken)
    {
        TransactionResult result = await _accountService.WithdrawAsync(
            CurrentAccountNumber,
            new Money(request.Amount),
            cancellationToken);

        return result switch
        {
            TransactionResult.Success => Ok(new { message = "Money withdrawn successfully" }),
            TransactionResult.Failure failure => BadRequest(new { failure.Code, failure.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(CancellationToken cancellationToken)
    {
        IReadOnlyList<Transaction> history = await _transactionService.GetHistoryAsync(
            CurrentAccountId,
            cancellationToken);

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
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _sessionService.LogoutAsync(CurrentSessionId, cancellationToken);

        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("debug/claims")]
    public IActionResult GetClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new { claims });
    }
}