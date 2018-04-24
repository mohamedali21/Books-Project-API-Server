using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace API_Server
{
    public class MyAuthinticationServerProvider : OAuthAuthorizationServerProvider
    {
        BooksEntities db = new BooksEntities();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user =
                (from u in db.Users
                 where u.Email == context.UserName && u.Pass == context.Password
                 select u).FirstOrDefault();

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (user != null)
            {
                if (user.Role == "user")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                    identity.AddClaim(new Claim("UserID", user.Id.ToString()));
                    identity.AddClaim(new Claim("email", user.Email));
                    identity.AddClaim(new Claim("role", user.Role));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    context.Validated(identity);
                }
                else if (user.Role == "admin")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                    identity.AddClaim(new Claim("email", user.Email));
                    identity.AddClaim(new Claim("UserID", user.Id.ToString()));
                    identity.AddClaim(new Claim("role", user.Role));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    context.Validated(identity);
                }
            }
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            //    identity.AddClaim(new Claim("email", user.Email));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            //    context.Validated(identity);
            //}
            //if (context.UserName == "admin" && context.Password == "admin")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            //    identity.AddClaim(new Claim("username", "admin"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "m_ali"));
            //    context.Validated(identity);
            //}
            //else if (context.UserName == "user" && context.Password == "user")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            //    identity.AddClaim(new Claim("username", "user"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "mahmoud hassan"));
            //    context.Validated(identity);
            //}
            else
            {
                context.SetError("Invalid_grant", "Provider username and password is incorrect");
            }
        }
    }
}