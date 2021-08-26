using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesAuth
{
    public class PostConfigureAccessTokenAuthenticationOptions : IPostConfigureOptions<AccessTokenAuthenticationOptions>
    {
        private readonly IAccessTokenTicketProvider accessTokenTicketProvider;

        public PostConfigureAccessTokenAuthenticationOptions(IAccessTokenTicketProvider accessTokenTicketProvider)
        {
            this.accessTokenTicketProvider = accessTokenTicketProvider;
        }

        public void PostConfigure(string name, AccessTokenAuthenticationOptions options)
        {
            options.TicketProvider = options.TicketProvider ?? accessTokenTicketProvider;
        }
    }
}
