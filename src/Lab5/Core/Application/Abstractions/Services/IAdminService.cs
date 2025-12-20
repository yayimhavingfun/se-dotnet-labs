using Core.Domain.Results;
using Core.Domain.ValueObjects;

namespace Core.Application.Abstractions.Services;

public interface IAdminService
{
    Task<AccountResult> CreateAccountAsync(
        AccountNumber number,
        string plainPin,
        Money initialDeposit,
        CancellationToken ct);
}