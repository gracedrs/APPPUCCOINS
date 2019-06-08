using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP_PUCCOINS.Models
{
    public class Transferencia
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }

        public string DataView { get; set; }

        public double Valor { get; set; }
        public string NomeUsuarioDestino { get; set; }
        public string NomeUsuarioOrigem { get; set; }
        public string Descricao { get; set; }
        public int ContaOrigemId { get; set; }
        public int ContaDestinoId { get; set; }
    }
}