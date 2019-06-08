using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class UsuarioPremio
    {
        [Key]
        [Column(Order = 0)]
        [Index("IX_Usuario_Premio", 1, IsUnique = true)]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [Key]
        [Column(Order = 1)]
        [Index("IX_Usuario_Premio", 2, IsUnique = true)]
        [ForeignKey("Premio")]
        public int PremioId { get; set; }

        [Key]
        [Column(Order = 2)]
        [Index("IX_Usuario_Premio", 3, IsUnique = true)]
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataEntrega { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Usuario Usuario { get; set; }
        public virtual Premio Premio { get; set; }

    }
}