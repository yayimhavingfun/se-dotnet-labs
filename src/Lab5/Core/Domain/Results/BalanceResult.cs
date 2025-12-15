using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;

public abstract record BalanceResult
{
    private BalanceResult() { }

    public sealed record Success(Money Balance) : BalanceResult;

    public sealed record Failure(string Code, string Message) : BalanceResult;
}