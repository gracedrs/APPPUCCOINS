using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class Transferencia
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public int ContaOrigemId {get;set;}
        public int ContaDestinoId { get; set; }
        public string Descricao { get; set; }

        [ForeignKey("ContaOrigemId")]
        public Conta ContaOrigem { get; set; }

        [ForeignKey("ContaDestinoId")]
        public Conta ContaDestino { get; set; }
        
    }
}