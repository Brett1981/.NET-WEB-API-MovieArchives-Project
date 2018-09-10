using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FilmArsivProje.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace FilmArsivProje
{
    public class ApplicationOAuthProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
           FilmProjesiDatabaseEntities  db = new FilmProjesiDatabaseEntities();
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            Kullanicilar kullanicilar = db.Kullanicilar.FirstOrDefault(x => x.kullaniciadi == context.UserName && x.kullanicisifre == context.Password);

            if (kullanicilar != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("kullaniciid", Convert.ToString(kullanicilar.kullaniciid)));
                identity.AddClaim(new Claim("kullaniciadi", kullanicilar.kullaniciadi));
               
                var userRoles = kullanicilar.kullaniciyetki;
                string[] roleArray = new string[] { userRoles };
                foreach (string item in roleArray)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, item));

                }
                var additionalData = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "role",Newtonsoft.Json.JsonConvert.SerializeObject(roleArray)
                    }
                });
                var token = new AuthenticationTicket(identity, additionalData);
                context.Validated(token);
            }
            else
            {
                context.SetError("invalid_grant", "Kullanici adi veya sifre yanlis");
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}