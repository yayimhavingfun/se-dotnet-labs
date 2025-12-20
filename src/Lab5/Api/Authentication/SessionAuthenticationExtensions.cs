using Microsoft.AspNetCore.Authentication;

namespace Api.Authentication;

public static class SessionAuthenticationExtensions
{
    public static AuthenticationBuilder AddSessionAuthentication(this AuthenticationBuilder builder)
    {
        return builder.AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>(
            "SessionScheme",
            options => { });
    }
}