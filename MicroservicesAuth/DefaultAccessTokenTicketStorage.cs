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
        public IReadOnlyDictionary<string, AuthenticationTicket> Tickets => _cache;

        public DefaultAccessTokenTicketStorage()
        {
            _cache = new ConcurrentDictionary<string, AuthenticationTicket>();
        }

        public bool Contains(string accessToken) => _cache.ContainsKey(accessToken);

        public AuthenticationTicket GetTicket(string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken) && _cache.TryGetValue(accessToken, out var ticket))
                return ticket;
            else
                return null;
        }

        public void SetTicket(string accessToken, AuthenticationTicket ticket)
        {
            _cache.TryAdd(accessToken, ticket);
        }

        public void RemoveTicket(string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
                _cache.TryRemove(accessToken, out var ticket);
        }
    }
}
