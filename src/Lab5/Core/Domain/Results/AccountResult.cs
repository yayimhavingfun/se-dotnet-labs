using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;

public abstract record AccountResult
{
    private AccountResult() { }

    public sealed record Success(AccountNumber AccountNumber) : AccountResult;

    public sealed record Failure(string Code, string Message) : AccountResult;
}