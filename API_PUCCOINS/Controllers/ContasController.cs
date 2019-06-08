using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using API_PUCCOINS.Filters;
using API_PUCCOINS.Models;

namespace API_PUCCOINS.Controllers
{
    [BasicAuthentication]
    public class ContasController : ApiController
    {
        private API_PUCCOINSContext db = new API_PUCCOINSContext();

        // GET: api/Contas
        [AuthorizeUser(Roles = "Admin")]
        public IQueryable<Conta> GetContas()
        {
            return db.Contas;
        }

        // GET: api/Contas/5
        [AuthorizeUser(Roles = "Admin")]
        [ResponseType(typeof(Conta))]
        public IHttpActionResult GetConta(int id)
        {
            Conta conta = db.Contas.Find(id);
            if (conta == null)
            {
                return NotFound();
            }

            return Ok(conta);
        }

        // PUT: api/Contas/5
        [AuthorizeUser(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConta(int id, Conta conta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != conta.Id)
            {
                return BadRequest();
            }

            db.Entry(conta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContaExists(id))
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

        // POST: api/Contas
        [AuthorizeUser(Roles = "Admin")]
        [ResponseType(typeof(Conta))]
        public IHttpActionResult PostConta(Conta conta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contas.Add(conta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = conta.Id }, conta);
        }

        // DELETE: api/Contas/5
        [AuthorizeUser(Roles = "Admin")]
        [ResponseType(typeof(Conta))]
        public IHttpActionResult DeleteConta(int id)
        {
            Conta conta = db.Contas.Find(id);
            if (conta == null)
            {
                return NotFound();
            }

            db.Contas.Remove(conta);
            db.SaveChanges();

            return Ok(conta);
        }

        

        [Route("api/GetSaldo")]
        [HttpGet]
        [ResponseType(typeof(void))]
        public IHttpActionResult GetSaldo()
        {
            int idUser = Convert.ToInt32(Thread.CurrentPrincipal.Identity.Name);
            Conta conta = db.Contas.Where(a => a.UsuarioId == idUser).FirstOrDefault();

            if (conta == null)
            {
                return NotFound();
            }

            return Ok(conta);
        }

        [Route("api/GetContaDestino/{id}")]
        [HttpGet]
        [ResponseType(typeof(void))]
        public IHttpActionResult GetContaDestino(int id)
        {
            Conta conta = db.Contas.Where(a => a.UsuarioId == id).FirstOrDefault();

            if (conta == null)
            {
                return NotFound();
            }

            conta.Saldo = 0;

            return Ok(conta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContaExists(int id)
        {
            return db.Contas.Count(e => e.Id == id) > 0;
        }
    }
}