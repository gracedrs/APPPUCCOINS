using API_PUCCOINS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API_PUCCOINS.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;
            API_PUCCOINSContext db = new API_PUCCOINSContext();
            bool isValid;
                     
            if (authHeader != null)
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                var email = usernamePasswordArray[0];
                var password = CriptografiaMD5.GerarHashMd5(usernamePasswordArray[1]);

                // Replace this with your own system of security / means of validating credentials
                //var isValid = userName == "joao.silva" && password == "1234";


                UsuarioLogin usuario = db.Usuarios.Select(b => new UsuarioLogin()
                {
                    Id = b.Id,
                    Email = b.Email,
                    Senha = b.Senha

                }).SingleOrDefault( a => a.Email == email && a.Senha == password);

                if (usuario == null)
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }

                if (isValid)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(usuario.Id.ToString()), null);
                    Thread.CurrentPrincipal = principal;
                    return;
                }
            }

            HandleUnathorized(actionContext);
        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
        }
    }
}