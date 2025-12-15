using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;

public abstract record TransactionResult
{
    private TransactionResult() { }

    public sealed record Success(Transaction Transaction) : TransactionResult;

    public sealed record Failure(string Code, string Message) : TransactionResult;
}