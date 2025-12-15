using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Authentication;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.ObjectOrientedProgramming.Lab5.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly ICurrentSessionService _currentSession;

    public AdminController(
        IAdminService adminService,
        ICurrentSessionService currentSession)
    {
        _adminService = adminService;
        _currentSession = currentSession;
    }

    public record CreateAccountRequest(string AccountNumber, string Pin, decimal InitialBalance = 0);

    public record CreateAccountResponse(string AccountNumber);

    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        IActionResult? adminCheck = RequireAdmin();
        if (adminCheck is not null) return adminCheck;

        var initialDeposit = new Money(request.InitialBalance);

        AccountResult result = await _adminService.CreateAccountAsync(
            new AccountNumber(request.AccountNumber),
            request.Pin,
            initialDeposit);

        return result switch
        {
            AccountResult.Success success => Ok(new CreateAccountResponse(success.AccountNumber.Value)),
            AccountResult.Failure failure => BadRequest(new { failure.Code, failure.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    private UnauthorizedObjectResult? RequireAdmin()
    {
        AtmSession? session = _currentSession.CurrentSession;

        if (session is null || !session.IsAdmin)
            return Unauthorized(new { message = "Admin session is required" });

        return null;
    }
}