using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroservicesAuth
{
    public class DefaultAccessTokenTicketStorage : IAccessTokenTicketStorage
    {
        private readonly ConcurrentDictionary<string, AuthenticationTicket> _cache;

        public DefaultAccessTokenTicketStorage()
        {
            _cache = new ConcurrentDictionary<string, AuthenticationTicket>();
        }

        public Task<AuthenticationTicket> GetTicketAsync(string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken) && _cache.TryGetValue(accessToken, out var ticket))
                return Task.FromResult(ticket);
            else
                return Task.FromResult<AuthenticationTicket>(null);
        }

        public Task SetTicketAsync(string accessToken, AuthenticationTicket ticket)
        {
            _cache.TryAdd(accessToken, ticket);
            return Task.CompletedTask;
        }

        public Task RemoveTicketAsync(string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
                _cache.TryRemove(accessToken, out var ticket);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyDictionary<string, AuthenticationTicket>> GetTicketsAllAsync() =>
            Task.FromResult<IReadOnlyDictionary<string, AuthenticationTicket>>(_cache);
    }
}
