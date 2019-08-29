﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Providers;

public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
{
    protected override Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (userName != "testuser" || password != "Pass1word")
        {
            // No user with userName/password exists.
            return null;
        }

        // Create a ClaimsIdentity with all the claims for this user.
        Claim nameClaim = new Claim(ClaimTypes.Name, userName);
        List<Claim> claims = new List<Claim> { nameClaim };

        // important to set the identity this way, otherwise IsAuthenticated will be false
        // see: http://leastprivilege.com/2012/09/24/claimsidentity-isauthenticated-and-authenticationtype-in-net-4-5/
        ClaimsIdentity identity = new ClaimsIdentity(claims, AuthenticationTypes.Basic);

        var principal = new ClaimsPrincipal(identity);
        return new Task<IPrincipal>(()=> { return principal; });
    }
}