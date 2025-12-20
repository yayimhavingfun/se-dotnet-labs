using Core.Domain.ValueObjects;

namespace Core.Domain.Results;

public abstract record BalanceResult
{
    private BalanceResult() { }

    public sealed record Success(Money Balance) : BalanceResult;

    public sealed record Failure(string Code, string Message) : BalanceResult;
}