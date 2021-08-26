using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroservicesAuth
{
    public interface IAccessTokenTicketProvider
    {
        /// <summary>
        /// Запрос тикета (билета) аутентификации по токену
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<AuthenticationTicket> GetTicketAsync(string accessToken);
    }
}
