using Core.Application.Abstractions.Authentication;
using Core.Application.Abstractions.Services;
using Core.Domain.Results;
using Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IHashingService _hashingService;

    public AdminController(IAdminService adminService, IHashingService hashingService)
    {
        _adminService = adminService;
        _hashingService = hashingService;
    }

    private bool IsAdmin => User.FindFirst("IsAdmin")?.Value == "True";

    public record CreateAccountRequest(string AccountNumber, string Pin, decimal InitialBalance = 0);

    public record CreateAccountResponse(string AccountNumber);

    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount(
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        if (!IsAdmin)
            return Unauthorized(new { message = "Admin access required" });

        string pinHash = _hashingService.Hash(request.Pin);
        var initialDeposit = new Money(request.InitialBalance);

        AccountResult result = await _adminService.CreateAccountAsync(
            new AccountNumber(request.AccountNumber),
            pinHash,
            initialDeposit,
            cancellationToken);

        return result switch
        {
            AccountResult.Success success => Ok(new CreateAccountResponse(success.AccountNumber.Value)),
            AccountResult.Failure failure => BadRequest(new { failure.Code, failure.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }
}