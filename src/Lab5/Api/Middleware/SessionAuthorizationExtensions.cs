namespace Itmo.ObjectOrientedProgramming.Lab5.Api.Middleware;

public static class SessionAuthorizationExtensions
{
    public static IApplicationBuilder UseSessionAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SessionAuthorizationMiddleware>();
    }
}