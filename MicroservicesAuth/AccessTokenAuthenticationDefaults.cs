using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesAuth
{
    public static class AccessTokenAuthenticationDefaults
    {
        internal const string AuthorizationHeader = "Authorization";
        public const string AuthenticationScheme = "AccessToken";
        public const string AuthorizationScheme = "Bearer";
    }
}
