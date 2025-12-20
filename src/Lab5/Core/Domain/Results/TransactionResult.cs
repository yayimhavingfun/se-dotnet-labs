using Core.Domain.Entities;

namespace Core.Domain.Results;

public abstract record TransactionResult
{
    private TransactionResult() { }

    public sealed record Success(Transaction Transaction) : TransactionResult;

    public sealed record Failure(string Code, string Message) : TransactionResult;
}