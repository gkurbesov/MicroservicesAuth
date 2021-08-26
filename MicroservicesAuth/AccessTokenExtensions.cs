using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesAuth
{
    public static class AccessTokenExtensions
    {
        public static AuthenticationBuilder AddToken<T>(this AuthenticationBuilder builder) where T : class, IAccessTokenTicketProvider
            => builder.AddToken<T>(AccessTokenAuthenticationDefaults.AuthenticationScheme, null, null);

        public static AuthenticationBuilder AddToken<T>(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<AccessTokenAuthenticationOptions> configureOptions) where T : class, IAccessTokenTicketProvider
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IAccessTokenTicketProvider, T>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<AccessTokenAuthenticationOptions>, PostConfigureAccessTokenAuthenticationOptions>());
            builder.AddScheme<AccessTokenAuthenticationOptions, AccessTokenAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
            return builder;
        }
    }
}
