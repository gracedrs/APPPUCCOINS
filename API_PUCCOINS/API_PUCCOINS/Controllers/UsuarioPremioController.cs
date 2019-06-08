using API_PUCCOINS.Filters;
using API_PUCCOINS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_PUCCOINS.Controllers
{
    [BasicAuthentication]
    public class UsuarioPremioController : ApiController
    {
        private API_PUCCOINSContext db = new API_PUCCOINSContext();

        [Route("api/PremiosResgatados")]
        [HttpGet]
        public IQueryable<UsuarioPremioDTO> GetPremiosResgatados()
        {
            int idUser = Convert.ToInt32(Thread.CurrentPrincipal.Identity.Name);


            var premios = from b in db.UsuarioPremio
                          where b.UsuarioId == idUser
                          select new UsuarioPremioDTO()
                          {
                              UsuarioId = b.UsuarioId,
                              DataEntrega = b.DataEntrega,
                              DataSolicitacao = b.DataSolicitacao,
                              ValorPremio = b.Premio.Preco,
                              DescricaoPremio = b.Premio.Descricao,
                              PremioId = b.Premio.Id
                          };

            return premios;

            
        }

        [Route("api/ResgataPremio")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult ResgataPremio(UsuarioPremio usuarioPremio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            usuarioPremio.DataSolicitacao = DateTime.Now;
            usuarioPremio.DataEntrega = DateTime.Now.AddDays(10);

            int idUser = Convert.ToInt32(Thread.CurrentPrincipal.Identity.Name);

            Premio premio = db.Premios.AsNoTracking().Where(a => a.Id == usuarioPremio.PremioId).FirstOrDefault();
            Conta conta = db.Contas.AsNoTracking().Where(a => a.UsuarioId == idUser).FirstOrDefault();
            
            if(conta.Saldo < premio.Preco)
            {
               return BadRequest("Saldo Insuficiente");
            }

            usuarioPremio.UsuarioId = idUser;

            db.UsuarioPremio.Add(usuarioPremio);
            db.SaveChanges();

            DiminuiSaldo(conta, premio.Preco);
            DiminuiQuantidade(premio);

            return Ok("Prêmio resgatado!");

        }

        private void DiminuiSaldo(Conta conta, double preco)
        {
            conta.Saldo -= preco;

            db.Entry(conta).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void DiminuiQuantidade(Premio premio)
        {
            premio.Quantidade--;
            db.Entry(premio).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}