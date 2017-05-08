
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;


namespace MemicAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = IdentityServerConfig.Endpoint.IdentityUri, 
                RequiredScopes = new[] { "memicApi" }
            });

            // web api configuration
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }

    




}