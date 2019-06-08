using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace API_PUCCOINS.Models
{
    public class API_PUCCOINSContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public API_PUCCOINSContext() : base("name=API_PUCCOINSContext")
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            this.Configuration.LazyLoadingEnabled = false;
        }

        public System.Data.Entity.DbSet<API_PUCCOINS.Models.Usuario> Usuarios { get; set; }
        public System.Data.Entity.DbSet<API_PUCCOINS.Models.Conta> Contas { get; set; }
        public System.Data.Entity.DbSet<API_PUCCOINS.Models.Premio> Premios { get; set; }
        public System.Data.Entity.DbSet<API_PUCCOINS.Models.UsuarioPremio> UsuarioPremio { get; set; }
        public System.Data.Entity.DbSet<API_PUCCOINS.Models.Transferencia> Transferencias { get; set; }

        public System.Data.Entity.DbSet<API_PUCCOINS.Models.Permissao> Permissoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
