namespace API_PUCCOINS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Versao2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UsuarioPremio", "IX_Usuario_Premio");
            CreateIndex("dbo.UsuarioPremio", "UsuarioId");
            CreateIndex("dbo.UsuarioPremio", "PremioId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UsuarioPremio", new[] { "PremioId" });
            DropIndex("dbo.UsuarioPremio", new[] { "UsuarioId" });
            CreateIndex("dbo.UsuarioPremio", new[] { "UsuarioId", "PremioId" }, name: "IX_Usuario_Premio");
        }
    }
}
