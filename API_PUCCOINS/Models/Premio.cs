﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class Premio
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
	    public string Imagem { get; set; }
	    public int Quantidade { get; set; }
    }
}