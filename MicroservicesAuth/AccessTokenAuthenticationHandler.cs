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
        public AccessTokenAuthenticationHandler(IOptionsMonitor<AccessTokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Заголовок с данными для аутентификации не найден
            if (!Request.Headers.ContainsKey(AccessTokenAuthenticationDefaults.AuthorizationHeader))
                return AuthenticateResult.Fail("Missing Authorization Header");

            // получаем значение заголовка
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[AccessTokenAuthenticationDefaults.AuthorizationHeader]);

            // проверяем префикс токена (схема аутентификации, по умолчанию Bearer)
            if (!authHeader.Scheme.Equals(Options.AuthorizationScheme, StringComparison.InvariantCultureIgnoreCase))
                return AuthenticateResult.Fail("Invalid authorization scheme");

            var token = authHeader.Parameter;

            // проверяем наличие токена
            if (string.IsNullOrWhiteSpace(token))
                return AuthenticateResult.Fail("Authorization token is empty");

            // запрашиваем AuthenticationTicket (билет аутентификации из хранилища)
            var ticket = await Options.TicketProvider.GetTicketAsync(token);

            if (ticket != null)
            {
                // если есть тикет то проверяем не истек ли срок жизни
                if (ticket.Properties.ExpiresUtc.HasValue && ticket.Properties.ExpiresUtc.Value < Clock.UtcNow)
                    // время тикета истекло
                    return AuthenticateResult.Fail("Token is expired");
                else
                    return AuthenticateResult.Success(ticket);
            }
            else
            {
                // билет отсутствует (истекло время токена или не был предварительно авторизован)
                return AuthenticateResult.Fail("Failed to match token");
            }
        }
    }
}
