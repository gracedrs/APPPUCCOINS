﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class TransferenciaDTO
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public string NomeUsuarioDestino  { get; set; }
        public string NomeUsuarioOrigem { get; set; }
        public string Descricao { get; set; }
        public int NumContaOrigemId { get; set; }
        public int NumContaDestinoId { get; set; }
    }
}