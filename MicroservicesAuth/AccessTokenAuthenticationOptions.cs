using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesAuth
{
    public class AccessTokenAuthenticationOptions : Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions
    {
        public string AuthorizationScheme { get; set; }

        public AccessTokenAuthenticationOptions() : base()
        {
            AuthorizationScheme = AccessTokenAuthenticationDefaults.AuthorizationScheme;
        }
    }
}
