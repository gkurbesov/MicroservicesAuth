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
        IReadOnlyDictionary<string, AuthenticationTicket> Tickets { get; }
        bool Contains(string accessToken);
        AuthenticationTicket GetTicket(string accessToken);
        void SetTicket(string accessToken, AuthenticationTicket ticket);
        void RemoveTicket(string accessToken);
    }
}
