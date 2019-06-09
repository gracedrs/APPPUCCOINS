using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP_PUCCOINS_oAUTH.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public double Saldo { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

    }
}