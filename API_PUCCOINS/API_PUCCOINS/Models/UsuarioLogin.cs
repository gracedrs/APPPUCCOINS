using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class UsuarioLogin
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}