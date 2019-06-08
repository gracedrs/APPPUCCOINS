using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public Guid Guid { get; set; }
        public int PermissaoId { get; set; }
        public Permissao Permissao { get; set; }
        
    }
}