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
    public class TransferenciasController : ApiController
    {
        private API_PUCCOINSContext db = new API_PUCCOINSContext();


        // GET: api/Transferencias
        public IQueryable<TransferenciaDTO> GetTransferencias()
        {
            int idUser = Convert.ToInt32(Thread.CurrentPrincipal.Identity.Name);

            var transferencias = from b in db.Transferencias where b.ContaOrigem.UsuarioId == idUser || b.ContaDestino.UsuarioId == idUser
                                 select new TransferenciaDTO()
                                 {
                                     Id = b.Id,
                                     Data = b.Data,
                                     Valor = b.Valor,
                                     Descricao = b.Descricao,
                                     NomeUsuarioOrigem = b.ContaOrigem.Usuario.Nome,
                                     NomeUsuarioDestino = b.ContaDestino.Usuario.Nome,
                                     NumContaOrigemId = b.ContaOrigemId,
                                     NumContaDestinoId = b.ContaDestinoId
                                 };

            return transferencias;
        }

        // GET: api/Transferencias/5
        [ResponseType(typeof(TransferenciaDTO))]
        public IHttpActionResult GetTransferencia(int id)
        {
            int idUser = Convert.ToInt32(Thread.CurrentPrincipal.Identity.Name);

            var transferencia = from b in db.Transferencias where (b.ContaOrigem.UsuarioId == idUser || b.ContaDestino.UsuarioId == idUser) && b.Id == id
                                      select new TransferenciaDTO()
                                      {
                                          Id = b.Id,
                                          Data = b.Data,
                                          Valor = b.Valor,
                                          Descricao = b.Descricao,
                                          NomeUsuarioOrigem = b.ContaOrigem.Usuario.Nome,
                                          NomeUsuarioDestino = b.ContaDestino.Usuario.Nome,
                                          NumContaOrigemId = b.ContaOrigemId,
                                          NumContaDestinoId = b.ContaDestinoId
                                      };

            if (transferencia.Count() == 0)
            {
                return NotFound();
            }

            return Ok(transferencia);
        }

        // PUT: api/Transferencias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransferencia(int id, Transferencia transferencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transferencia.Id)
            {
                return BadRequest();
            }

            db.Entry(transferencia).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferenciaExists(id))
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

        // POST: api/Transferencias
        [ResponseType(typeof(Transferencia))]
        public IHttpActionResult PostTransferencia(Transferencia transferencia)
        {
            int idUser = Convert.ToInt32(Thread.CurrentPrincipal.Identity.Name);

            //Encontra a conta do usuário autor da transferência
            Conta contaOrigem = db.Contas.AsNoTracking().Where(a => a.UsuarioId == idUser).FirstOrDefault();

            if(contaOrigem.Saldo < transferencia.Valor)
            {
                return BadRequest("Saldo Insuficiente para Transferência");
            }

            transferencia.ContaOrigemId = contaOrigem.Id;
            
            transferencia.Data = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transferencias.Add(transferencia);
            db.SaveChanges();

            AtualizaSaldo(transferencia.ContaOrigemId, transferencia.ContaDestinoId, transferencia.Valor);

            return CreatedAtRoute("DefaultApi", new { id = transferencia.Id }, transferencia);
        }

        private void AtualizaSaldo(int contaOrigemId, int contaDestinoId, double valor)
        {
            Conta contaOrigem = db.Contas.Where(a => a.Id == contaOrigemId).FirstOrDefault();
            Conta contaDestino = db.Contas.Where(a => a.Id == contaDestinoId).FirstOrDefault();

            contaOrigem.Saldo -= valor;
            contaDestino.Saldo += valor;

            db.Entry(contaOrigem).State = EntityState.Modified;
            db.Entry(contaDestino).State = EntityState.Modified;

            db.SaveChanges();
        }

        // DELETE: api/Transferencias/5
        [ResponseType(typeof(Transferencia))]
        public IHttpActionResult DeleteTransferencia(int id)
        {
            Transferencia transferencia = db.Transferencias.Find(id);
            if (transferencia == null)
            {
                return NotFound();
            }

            db.Transferencias.Remove(transferencia);
            db.SaveChanges();

            return Ok(transferencia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransferenciaExists(int id)
        {
            return db.Transferencias.Count(e => e.Id == id) > 0;
        }
    }
}