using APP_PUCCOINS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace APP_PUCCOINS.Controllers
{
    public class LoginController : Controller
    {
        HttpClient client = new HttpClient();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Usuario u)
        {
            if (ModelState.IsValid) 
            {
                using (var client = new HttpClient())
                {
                    string url  = "https://apipuccoins.azurewebsites.net/api/DoLogin";
                    var uri = new Uri(url);
                    var data = JsonConvert.SerializeObject(u);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, content).Result;

                    

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync();
                        Usuario userLogado = JsonConvert.DeserializeObject<Usuario>(json.Result);
                        u.Id = userLogado.Id;
                        this.Session["UserProfile"] = u;
                        return RedirectToAction("Index", "Paginas");
                    }
                }
            }
            return View(u);
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Logout()
        {
            Session["UserProfile"] = null;
            return RedirectToAction("Index");
        }

    }
}