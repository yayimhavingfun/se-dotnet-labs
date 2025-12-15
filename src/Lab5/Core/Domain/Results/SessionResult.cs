using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;

public abstract record SessionResult
{
    private SessionResult() { }

    public sealed record Success(AtmSession AtmSession) : SessionResult;

    public sealed record Failure(string Code, string Message) : SessionResult;
}