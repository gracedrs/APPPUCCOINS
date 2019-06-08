using API_PUCCOINS.Filters;
using API_PUCCOINS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_PUCCOINS.Controllers
{

    public class LoginController : ApiController
    {
        private API_PUCCOINSContext db = new API_PUCCOINSContext();

        [Route("api/DoLogin")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult DoLogin(Usuario usuario)
        {
            string senhaEncrypt = CriptografiaMD5.GerarHashMd5(usuario.Senha);
            Usuario user = db.Usuarios.Where(a => a.Email == usuario.Email && a.Senha == senhaEncrypt).FirstOrDefault();
            if(user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Usuário ou senha Incorreto");
            }
        }

        [Route("api/NovoUsuario")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            usuario.Guid = Guid.NewGuid(); //GUID para lógica da recuperação de senha via email
            usuario.PermissaoId = 2; //Permissão de Usuário normal
            usuario.Senha = CriptografiaMD5.GerarHashMd5(usuario.Senha);
            db.Usuarios.Add(usuario);
            db.SaveChanges();

            CriarConta(usuario.Id);

            return CreatedAtRoute("DefaultApi", new { id = usuario.Id }, usuario);
        }

        private void CriarConta(int usuarioId)
        {
            Conta novaConta = new Conta
            {
                Saldo = 0,
                UsuarioId = usuarioId
            };

            db.Entry(novaConta).State = EntityState.Added;

            db.SaveChanges();
        }

        [Route("api/ModificaSenha/{guid}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult ModificaSenha(string guid, Usuario usuario)
        {
            Usuario user = db.Usuarios.AsNoTracking().Where(a => a.Guid.ToString() == guid).SingleOrDefault();

            if (user == null)
            {
                return BadRequest("Erro na recuperação da senha.");
            }

            usuario.Email = user.Email;
            usuario.Id = user.Id;
            usuario.Nome = user.Nome;

            db.Entry(usuario).State = EntityState.Modified;

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
