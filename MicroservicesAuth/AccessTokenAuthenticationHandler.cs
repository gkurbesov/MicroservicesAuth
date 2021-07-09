using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MicroservicesAuth
{
    public class AccessTokenAuthenticationHandler : AuthenticationHandler<AccessTokenAuthenticationOptions>
    {
        private readonly IAccessTokenTicketStorage ticketStorage;

        public AccessTokenAuthenticationHandler(IOptionsMonitor<AccessTokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAccessTokenTicketStorage ticketStorage)
            : base(options, logger, encoder, clock)
        {
            this.ticketStorage = ticketStorage;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AccessTokenAuthenticationDefaults.AuthorizationHeader))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[AccessTokenAuthenticationDefaults.AuthorizationHeader]);

            if (!authHeader.Scheme.Equals(Options.AuthorizationScheme, StringComparison.InvariantCultureIgnoreCase))
                return AuthenticateResult.Fail("Invalid authorization scheme");

            if (string.IsNullOrWhiteSpace(authHeader.Parameter))
                return AuthenticateResult.Fail("Authorization token is empty");

            var ticket = ticketStorage.GetTicket(authHeader.Parameter);

            if (ticket != null)
            {
                if (ticket.Properties.ExpiresUtc.HasValue && ticket.Properties.ExpiresUtc.Value < Clock.UtcNow)
                {
                    ticketStorage.RemoveTicket(authHeader.Parameter);
                    return AuthenticateResult.Fail("Token is expired");
                }
                else
                {
                    return AuthenticateResult.Success(ticket);
                }
            }
            else
            {
                return AuthenticateResult.Fail("Failed to match token");
            }
        }
    }
}
