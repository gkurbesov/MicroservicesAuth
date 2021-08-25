using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesAuth
{
    /// <summary>
    /// Стандартные значения используемые в библиотеке
    /// </summary>
    public static class AccessTokenAuthenticationDefaults
    {
        /// <summary>
        /// Заголовок head'ра который содержит данные для аутентификации
        /// </summary>
        internal const string AuthorizationHeader = "Authorization";
        /// <summary>
        /// Название схемы аутентификации
        /// </summary>
        public const string AuthenticationScheme = "AccessToken";
        /// <summary>
        /// Префикс токена аутентификации
        /// </summary>
        public const string AuthorizationScheme = "Bearer";
    }
}
