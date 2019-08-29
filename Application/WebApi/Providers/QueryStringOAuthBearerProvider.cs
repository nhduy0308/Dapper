﻿using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace WebAPI.Providers
{
    public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Headers["access_token"];

            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }

            return Task.FromResult<object>(null);
        }
    }
}