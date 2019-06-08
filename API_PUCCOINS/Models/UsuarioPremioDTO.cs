using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class UsuarioPremioDTO
    {
        public int UsuarioId { get; set; }
        public int PremioId { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataEntrega { get; set; }

        public string DescricaoPremio { get; set; }
        public double ValorPremio { get; set; }
    }
}