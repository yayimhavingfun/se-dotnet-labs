using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Authentication;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;

namespace Itmo.ObjectOrientedProgramming.Lab5.Api.Middleware;

public class SessionAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public SessionAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ISessionRepository sessionRepository,
        ICurrentSessionService currentSessionService)
    {
        string? path = context.Request.Path.Value;
        if (path != null && (path.EndsWith("/auth/user") || path.EndsWith("/auth/admin")))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Session-Id", out Microsoft.Extensions.Primitives.StringValues headerValues) ||
            !Guid.TryParse(headerValues.FirstOrDefault(), out Guid sessionId))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("{\"message\": \"X-Session-Id is invalid or not found\"}");
            return;
        }

        Core.Domain.Entities.AtmSession? session = await sessionRepository.FindByIdAsync(sessionId, context.RequestAborted);

        if (session is null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("{\"message\": \"Session is not found\"}");
            return;
        }

        currentSessionService.SetCurrentSession(session);

        await _next(context);
    }
}