using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API_PUCCOINS.Filters;
using API_PUCCOINS.Models;

namespace API_PUCCOINS.Controllers
{
    [BasicAuthentication]
    public class PremiosController : ApiController
    {
        private API_PUCCOINSContext db = new API_PUCCOINSContext();

        // GET: api/Premios
        /// <summary>
        /// Retorna todos os Premios cadastrados
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        public IQueryable<Premio> GetPremios()
        {
            return db.Premios;
        }

        // GET: api/Premios/5
        /// <summary>
        /// Busca um premio atraves de ID especifico
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [ResponseType(typeof(Premio))]
        public IHttpActionResult GetPremio(int id)
        {
            Premio premio = db.Premios.Find(id);
            if (premio == null)
            {
                return NotFound();
            }

            return Ok(premio);
        }

        // PUT: api/Premios/5
        /// <summary>
        /// Atualiza os dados de um premio
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [AuthorizeUser(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPremio(int id, Premio premio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != premio.Id)
            {
                return BadRequest();
            }

            db.Entry(premio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PremioExists(id))
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

        // POST: api/Premios
        /// <summary>
        /// Cadastra um novo premio
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [AuthorizeUser(Roles = "Admin")]
        [ResponseType(typeof(Premio))]
        public IHttpActionResult PostPremio(Premio premio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Premios.Add(premio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = premio.Id }, premio);
        }

        // DELETE: api/Premios/5
        /// <summary>
        /// Deleta um premio atraves de seu ID
        /// </summary>
        /// <param name="pessoa">Objeto do tipo Pessoa</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">Ocorreu um erro na exceção</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="403">Acesso não autorizado</response>
        /// <returns></returns>
        [AuthorizeUser(Roles = "Admin")]
        [ResponseType(typeof(Premio))]
        public IHttpActionResult DeletePremio(int id)
        {
            Premio premio = db.Premios.Find(id);
            if (premio == null)
            {
                return NotFound();
            }

            db.Premios.Remove(premio);
            db.SaveChanges();

            return Ok(premio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PremioExists(int id)
        {
            return db.Premios.Count(e => e.Id == id) > 0;
        }
    }
}