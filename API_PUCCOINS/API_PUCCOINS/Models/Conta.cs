using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public double Saldo { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
    }
}