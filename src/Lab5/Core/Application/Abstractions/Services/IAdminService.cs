using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;

public interface IAdminService
{
    Task<AccountResult> CreateAccountAsync(
        AccountNumber number,
        string plainPin,
        Money initialDeposit,
        CancellationToken ct = default);
}