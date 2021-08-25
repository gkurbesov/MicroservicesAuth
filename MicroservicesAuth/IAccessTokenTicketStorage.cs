using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroservicesAuth
{
    public interface IAccessTokenTicketStorage
    {
        /// <summary>
        /// Запрос всего списка хранящихся токенов
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyDictionary<string, AuthenticationTicket>> GetTicketsAllAsync();
        /// <summary>
        /// Запрос тикета (билета) аутентификации по токену
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<AuthenticationTicket> GetTicketAsync(string accessToken);
        /// <summary>
        /// Добавить тикет аутентификации в хранилище
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        Task SetTicketAsync(string accessToken, AuthenticationTicket ticket);
        /// <summary>
        /// Удалить тикет аутентификации из хранилища
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task RemoveTicketAsync(string accessToken);
    }
}
