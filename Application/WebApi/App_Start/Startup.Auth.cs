﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using Data;
using Model.Models;
using WebAPI.Providers;
using Microsoft.Owin.Security.Infrastructure;

[assembly: OwinStartup(typeof(WebAPI.App_Start.Startup))]

namespace WebAPI.App_Start
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(DbContext.Create);

            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<UserManager<AspNetUser>>(CreateManager);

            //Allow Cross origin for API
            app.UseCors(CorsOptions.AllowAll);

          

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                AllowInsecureHttp = true
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var OAuthBearerOptions = new OAuthBearerAuthenticationOptions()
            {
                Provider = new QueryStringOAuthBearerProvider(),
                AccessTokenProvider = new AuthenticationTokenProvider()
                {
                    OnCreate = create,
                    OnReceive = receive
                },
            };

            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);

                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
                {
                    Provider = new QueryStringOAuthBearerProvider()
                });

                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    EnableJSONP = true,
                    EnableDetailedErrors = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });

        }
        private static UserManager<AspNetUser> CreateManager(IdentityFactoryOptions<UserManager<AspNetUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<AspNetUser>(context.Get<DbContext>());
            var owinManager = new UserManager<AspNetUser>(userStore);

            return owinManager;
        }
        private static Action<AuthenticationTokenCreateContext> create = new Action<AuthenticationTokenCreateContext>(c =>
        {
            c.SetToken(c.SerializeTicket());
        });

        private static Action<AuthenticationTokenReceiveContext> receive = new Action<AuthenticationTokenReceiveContext>(c =>
        {
            c.DeserializeTicket(c.Token);
            //c.OwinContext.Environment["Properties"] = c.Ticket.Properties;
        });
    }
}