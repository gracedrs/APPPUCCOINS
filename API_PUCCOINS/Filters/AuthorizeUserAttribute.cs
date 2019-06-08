using API_PUCCOINS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace API_PUCCOINS.Filters
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var roles = Roles;
            //var t = Thread.CurrentPrincipal;
            if (!UserAuthorized(Roles, Thread.CurrentPrincipal.Identity.Name))
            {
                return false;
            }

            return true;
        }

        private bool UserAuthorized(string roles, string id)
        {
            API_PUCCOINSContext db = new API_PUCCOINSContext();
            string[] vetRoles = roles.Split(',');
            int idUser = Convert.ToInt32(id);

            //TODO: Modificar regra de negocio de cargo
            var usuario = db.Usuarios.Include("Permissao").Where(a => a.Id == idUser).FirstOrDefault();
            
            foreach (string role in vetRoles)
            {
                if (role.Equals(usuario.Permissao.Descricao))
                {
                    return true;
                }
            }

            return false;
        }
    }
}