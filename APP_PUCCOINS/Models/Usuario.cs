using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APP_PUCCOINS.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe a senha do usuário", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        
        public string Senha { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Informe o email", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Email { get; set; }
    }
}