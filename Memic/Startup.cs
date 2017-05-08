using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using static IdentityServer3.Core.Constants;

namespace Memic
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Embedded IdentityServer",
                    SigningCertificate = LoadCertificate(),

                    Factory = new IdentityServerServiceFactory()
                                .UseInMemoryUsers(Config.Users.Get())
                                .UseInMemoryClients(Config.Clients.Get())
                                .UseInMemoryScopes(Config.Scopes.Get())
                });
            });
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });


            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
               
                Authority = IdentityServerConfig.Endpoint.IdentityUri,
                ClientId = "mvc",
                Scope = "openid profile roles",
               
                RedirectUri = IdentityServerConfig.Endpoint.IdentityServerBaseUri,
                ResponseType = "id_token",
               
                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        // we want to keep first name, last name, subject and roles
                        var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
                        var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
                        var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                        var roles = id.FindAll(Constants.ClaimTypes.Role);

                        // create new identity and set name and role claim type
                        var nid = new ClaimsIdentity(
                            id.AuthenticationType,
                            Constants.ClaimTypes.GivenName,
                            Constants.ClaimTypes.Role);
                        if (givenName != null)
                        {
                            nid.AddClaim(givenName);
                        }
                        if (familyName != null)
                        {
                            nid.AddClaim(familyName);
                        }
                        nid.AddClaim(sub);
                        nid.AddClaims(roles);

                        // add some other app specific claim
                        nid.AddClaim(new Claim("app_specific", "some data"));

                        // keep the id_token for logout
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));



                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);






                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }

                        return Task.FromResult(0);
                    }
                }
            });

            app.UseResourceAuthorization(new AuthorizationManager());



            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();


        }


        X509Certificate2 LoadCertificate()
        {
            string certpath = string.Format(@"{0}certificates\idsrv3test.pfx",
                AppDomain.CurrentDomain.BaseDirectory);

            return new X509Certificate2(certpath, "idsrv3test");
        }
    }


}