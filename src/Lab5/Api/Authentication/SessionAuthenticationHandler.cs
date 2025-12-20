using Core.Application.Abstractions.Repositories;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Api.Authentication;

public class SessionAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ISessionRepository _sessionRepository;

    public SessionAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISessionRepository sessionRepository)
        : base(options, logger, encoder)
    {
        _sessionRepository = sessionRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-Session-Id", out StringValues sessionIdValues) ||
            !Guid.TryParse(sessionIdValues.FirstOrDefault(), out Guid sessionId))
        {
            return AuthenticateResult.NoResult();
        }

        AtmSession? session = await _sessionRepository.FindByIdAsync(sessionId, Context.RequestAborted);

        if (session is null)
        {
            return AuthenticateResult.NoResult();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, session.SessionId.ToString()),
            new Claim("IsAdmin", session.IsAdmin.ToString()),
        };

        if (session.Account is not null && !session.IsAdmin)
        {
            claims.Add(new Claim("AccountId", session.Account.Id.ToString()));
            claims.Add(new Claim("AccountNumber", session.Account.AccountNumber.Value));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}