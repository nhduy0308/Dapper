using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace WebApi.Providers
{
    public abstract class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get { return false; } }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                return;
            }

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != "Basic")
            {
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            Tuple<string, string> userNameAndPassword = ExtractUserNameAndPassword(authorization.Parameter);
            if (userNameAndPassword == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
            }

            string userName = userNameAndPassword.Item1;
            string password = userNameAndPassword.Item2;

            IPrincipal principal = await AuthenticateAsync(userName, password, cancellationToken);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
            }

            // 6. If the credentials are valid, set principal.
            else
            {
                context.Principal = principal;
            }

        }

        protected abstract Task<IPrincipal> AuthenticateAsync(string userName, string password,CancellationToken cancellationToken);

        private Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            string userName = decodedCredentials.Substring(0, colonIndex);
            string password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName, password);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

    }
}