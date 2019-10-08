using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebAppAPI_Autorization.Models;

namespace WebAppAPI_Autorization
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.UseOAuthBearerTokens(OAuthOptions);
        }

        static Startup()
        {
            OAuthOptions = new OAuthAuthorizationServerOptions()
            {
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(7),
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                Provider = new OAuthProvider()
            };
        }
    }

    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                                             string username = context.UserName;
                                             string password = context.Password;

                                             Usuario usuario = new Usuario().ObterPor(username, password);

                                             if (usuario != null)
                                             {
                                                 List<Claim> calims = new List<Claim>
                                                                      {
                                                                          new Claim(ClaimTypes.Name, usuario.Nome),
                                                                          new Claim("UserId", usuario.Id.ToString()),
                                                                          new Claim(ClaimTypes.Role, usuario.Role)

                                                                      };
                                                 ClaimsIdentity oAuthIdentity = new ClaimsIdentity(calims,Startup.OAuthOptions.AuthenticationType);
                                                 context.Validated(new AuthenticationTicket(oAuthIdentity,
                                                                                            new
                                                                                            AuthenticationProperties()
                                                                                            {
                                                                                            }));

                                                 GenericPrincipal principal = new GenericPrincipal(oAuthIdentity, new string[] { });
                                                 Thread.CurrentPrincipal = principal;

                                                 


                                             }
                                             else
                                             {
                                                 context.SetError("invalid_grant", "Usuário ou senha inválidos");
                                             }

                                         });

           
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if(context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }
    }
}