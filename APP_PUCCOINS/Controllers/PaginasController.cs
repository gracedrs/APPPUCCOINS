using APP_PUCCOINS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace APP_PUCCOINS.Controllers
{

    public class PaginasController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            ViewBag.Saldo = GetSaldo();

            return View();
        }

        public ActionResult MinhasMoedas()
        {
            ViewBag.Title = "Home Page";

            ViewBag.Saldo = GetSaldo();

            return View();
        }

        public ActionResult Relatorio()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Resgatar()
        {
            ViewBag.Title = "Home Page";

            ViewBag.Saldo = GetSaldo();

            using (var client = new HttpClient())
            {
                string url = "https://apipuccoins.azurewebsites.net/api/Premios";

                Usuario usuario = (Usuario)Session["UserProfile"];

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usuario.Email, usuario.Senha))));
                var response = client.GetStringAsync(url).Result;

                List<Premio> premios = JsonConvert.DeserializeObject<List<Premio>>(response);

                ViewBag.Premios = new SelectList(premios, "Id", "Descricao");

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transferencias(Usuario u)
        {
            ViewBag.Title = "Home Page";

            using (var client = new HttpClient())
            {
                string url = "https://apipuccoins.azurewebsites.net/api/GetUsuariosTransferencia?query=" + u.Email;

                Usuario usuario = (Usuario)Session["UserProfile"];

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usuario.Email, usuario.Senha))));
                var response = client.GetStringAsync(url).Result;
                
                Usuario usuarioPesquisado = JsonConvert.DeserializeObject<Usuario>(response);

                if(usuarioPesquisado != null)
                {
                    u.Email = usuarioPesquisado.Email;
                    u.Id = usuarioPesquisado.Id;
                    u.Nome = usuarioPesquisado.Nome;
                    ViewBag.UsuarioPesquisado = u.Nome;
                }
                else
                {
                    ViewBag.UsuarioPesquisado = "Usuário não encontrado";
                }
            }
            
            return View(u);
        }

        public ActionResult Transferencias()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        protected override void OnActionExecuting(
        ActionExecutingContext filterContext)
        {
            if (Session["UserProfile"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller", "Login"},
                        {"action", "Index"}
                    }
                );
            }
            // code involving this.Session // edited to simplify
            base.OnActionExecuting(filterContext); // re-added in edit
        }

        [HttpGet]
        public ActionResult LoadExtrato()
        {
            using (var client = new HttpClient())
            {
                string url = "https://apipuccoins.azurewebsites.net/api/Transferencias";

                Usuario usuario = (Usuario)Session["UserProfile"];

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usuario.Email, usuario.Senha))));
                var response = client.GetStringAsync(url).Result;

                List<Transferencia> transferencias = JsonConvert.DeserializeObject<List<Transferencia>>(response);

                foreach(Transferencia t in transferencias)
                {
                    t.DataView = t.Data.ToString();
                }

                return Json(new { data = transferencias }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadPremiosResgatados()
        {
            using (var client = new HttpClient())
            {
                string url = "https://apipuccoins.azurewebsites.net/api/PremiosResgatados";

                Usuario usuario = (Usuario)Session["UserProfile"];

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usuario.Email, usuario.Senha))));
                var response = client.GetStringAsync(url).Result;

                List<UsuarioPremioDTO> usuarioPremios = JsonConvert.DeserializeObject<List<UsuarioPremioDTO>>(response);

                foreach(UsuarioPremioDTO up in usuarioPremios)
                {
                    up.DataEntregaView = up.DataEntrega.ToString();
                    up.DataSolicitacaoView = up.DataSolicitacao.ToString();
                };

                return Json(new { data = usuarioPremios }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string RealizaTransferencia(int idUser, double valor)
        {
            Usuario u = (Usuario)Session["UserProfile"];

            int idContaDestino = GetConta(idUser);
            int idContaOrigem = GetConta(u.Id);

            Transferencia transferencia = new Transferencia
            {
                ContaOrigemId = idContaOrigem,
                ContaDestinoId = idContaDestino,
                Valor = valor
            };

            using (var client = new HttpClient())
            {
                string url = "https://apipuccoins.azurewebsites.net/api/Transferencias";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", u.Email, u.Senha))));

                var uri = new Uri(url);
                var data = JsonConvert.SerializeObject(transferencia);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return "Transferência realizada com sucesso.";
                }
                else
                {
                    return "Erro na transferência";
                }
            }
        }


        [HttpPost]
        public string SolicitaResgatePremio(int idPremio)
        {
            Usuario u = (Usuario)Session["UserProfile"];
            UsuarioPremio usuarioPremio = new UsuarioPremio
            {
                UsuarioId = u.Id,
                PremioId = idPremio
            };

            using (var client = new HttpClient())
            {
                string url = "https://apipuccoins.azurewebsites.net/api/ResgataPremio";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", u.Email, u.Senha))));

                var uri = new Uri(url);
                var data = JsonConvert.SerializeObject(usuarioPremio);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return "Pedido de Resgate de Prêmio realizado com sucesso.";
                }
                else
                {
                    return "Erro no resgate de Prêmio";
                }
            }
        }

        public double GetSaldo()
        {
            using (var client = new HttpClient())
            {
                string url = "https://apipuccoins.azurewebsites.net/api/GetSaldo";

                Usuario usuario = (Usuario)Session["UserProfile"];

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usuario.Email, usuario.Senha))));
                var response = client.GetStringAsync(url).Result;

                Conta conta = JsonConvert.DeserializeObject<Conta>(response);

                return conta.Saldo;

            }
        }


        public int GetConta(int idUser)
        {
            using (var client = new HttpClient())
            {
                string url = "https://apipuccoins.azurewebsites.net/api/GetSaldo";

                Usuario usuario = (Usuario)Session["UserProfile"];

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usuario.Email, usuario.Senha))));
                var response = client.GetStringAsync(url).Result;

                Conta conta = JsonConvert.DeserializeObject<Conta>(response);

                return conta.Id;

            }
        }

    }
}