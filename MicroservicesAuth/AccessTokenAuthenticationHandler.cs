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
            // Заголовок с данными для аутентификации не найден
            if (!Request.Headers.ContainsKey(AccessTokenAuthenticationDefaults.AuthorizationHeader))
                return AuthenticateResult.Fail("Missing Authorization Header");

            // получаем значение заголовка
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[AccessTokenAuthenticationDefaults.AuthorizationHeader]);

            // проверяем префикс токена (схема аутентификации, по умолчанию Bearer)
            if (!authHeader.Scheme.Equals(Options.AuthorizationScheme, StringComparison.InvariantCultureIgnoreCase))
                return AuthenticateResult.Fail("Invalid authorization scheme");

            // проверяем наличие токена
            if (string.IsNullOrWhiteSpace(authHeader.Parameter))
                return AuthenticateResult.Fail("Authorization token is empty");

            // запрашиваем AuthenticationTicket (билет аутентификации из хранилища)
            var ticket = await ticketStorage.GetTicketAsync(authHeader.Parameter);

            if (ticket != null)
            {
                // если есть тикет то проверяем не истек ли срок жизни
                if (ticket.Properties.ExpiresUtc.HasValue && ticket.Properties.ExpiresUtc.Value < Clock.UtcNow)
                {
                    // время тикета истекло - удаляем
                    await ticketStorage.RemoveTicketAsync(authHeader.Parameter);
                    return AuthenticateResult.Fail("Token is expired");
                }
                else
                {
                    return AuthenticateResult.Success(ticket);
                }
            }
            else
            {
                // билет отсутствует (истекло время токена или не был предварительно авторизован)
                return AuthenticateResult.Fail("Failed to match token");
            }
        }
    }
}
