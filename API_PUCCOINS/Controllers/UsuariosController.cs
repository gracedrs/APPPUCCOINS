using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using API_PUCCOINS.Filters;
using API_PUCCOINS.Models;

namespace API_PUCCOINS.Controllers
{
    [BasicAuthentication]
    
    public class UsuariosController : ApiController
    {
        private API_PUCCOINSContext db = new API_PUCCOINSContext();

        [AuthorizeUser(Roles = "Admin")]
        // GET: api/Usuarios
        /// <summary>
        /// Retorna todos os usuários cadastrados
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        public IQueryable<Usuario> GetUsuarios()
        {
            return db.Usuarios;
        }

        /// <summary>
        /// Busca usuário para transferencia
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [Route("api/GetUsuariosTransferencia")]
        [HttpGet]
        [ResponseType(typeof(UsuarioDTO))]
        public IHttpActionResult GetUsuarioTransferencia(string query)
        {
            UsuarioDTO usuario = (from b in db.Usuarios
                                 where b.Email.Equals(query) || b.Nome.Equals(query)
                                 select new UsuarioDTO()
                                 {
                                     Id = b.Id,
                                     Nome = b.Nome,
                                     Email = b.Email
                                 }).FirstOrDefault();


            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // GET: api/Usuarios/5
        /// <summary>
        /// Busca um usuário com o ID específico
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [AuthorizeUser(Roles = "Admin")]
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult GetUsuario(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        /// <summary>
        /// Retorna o usuário através do email
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [Route("api/GetUsuarioByEmail")]
        [AuthorizeUser(Roles = "Admin")]
        // GET: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult GetUsuarioByEmail(string email)
        {
            Usuario usuario = db.Usuarios.Where(a => a.Email == email).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        /// <summary>
        /// Atualiza as informações de um usuário através de um ID
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [AuthorizeUser(Roles = "Admin")]
        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuario(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.Id)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Insere um novo usuário
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [AuthorizeUser(Roles = "Admin")]
        // POST: api/Usuarios
        [ResponseType(typeof(Usuario))]
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

        /// <summary>
        /// Remove um usuário através de um ID especificado
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [AuthorizeUser(Roles = "Admin")]
        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult DeleteUsuario(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuarios.Count(e => e.Id == id) > 0;
        }
    }
}