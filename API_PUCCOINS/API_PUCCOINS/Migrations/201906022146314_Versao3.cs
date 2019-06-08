namespace API_PUCCOINS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Versao3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UsuarioPremio", new[] { "UsuarioId" });
            DropIndex("dbo.UsuarioPremio", new[] { "PremioId" });
            DropPrimaryKey("dbo.UsuarioPremio");
            AddPrimaryKey("dbo.UsuarioPremio", new[] { "UsuarioId", "PremioId", "DataSolicitacao" });
            CreateIndex("dbo.UsuarioPremio", new[] { "UsuarioId", "PremioId", "DataSolicitacao" }, unique: true, name: "IX_Usuario_Premio");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UsuarioPremio", "IX_Usuario_Premio");
            DropPrimaryKey("dbo.UsuarioPremio");
            AddPrimaryKey("dbo.UsuarioPremio", new[] { "UsuarioId", "PremioId" });
            CreateIndex("dbo.UsuarioPremio", "PremioId");
            CreateIndex("dbo.UsuarioPremio", "UsuarioId");
        }
    }
}
