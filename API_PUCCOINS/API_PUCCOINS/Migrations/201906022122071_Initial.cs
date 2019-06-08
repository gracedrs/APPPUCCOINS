namespace API_PUCCOINS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Saldo = c.Double(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Senha = c.String(),
                        Email = c.String(),
                        Guid = c.Guid(nullable: false),
                        PermissaoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissao", t => t.PermissaoId)
                .Index(t => t.PermissaoId);
            
            CreateTable(
                "dbo.Permissao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Premio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        Preco = c.Double(nullable: false),
                        Imagem = c.String(),
                        Quantidade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transferencia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Valor = c.Double(nullable: false),
                        ContaOrigemId = c.Int(nullable: false),
                        ContaDestinoId = c.Int(nullable: false),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conta", t => t.ContaDestinoId)
                .ForeignKey("dbo.Conta", t => t.ContaOrigemId)
                .Index(t => t.ContaOrigemId)
                .Index(t => t.ContaDestinoId);
            
            CreateTable(
                "dbo.UsuarioPremio",
                c => new
                    {
                        UsuarioId = c.Int(nullable: false),
                        PremioId = c.Int(nullable: false),
                        DataSolicitacao = c.DateTime(nullable: false),
                        DataEntrega = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioId, t.PremioId })
                .ForeignKey("dbo.Premio", t => t.PremioId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => new { t.UsuarioId, t.PremioId }, name: "IX_Usuario_Premio");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuarioPremio", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioPremio", "PremioId", "dbo.Premio");
            DropForeignKey("dbo.Transferencia", "ContaOrigemId", "dbo.Conta");
            DropForeignKey("dbo.Transferencia", "ContaDestinoId", "dbo.Conta");
            DropForeignKey("dbo.Conta", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Usuario", "PermissaoId", "dbo.Permissao");
            DropIndex("dbo.UsuarioPremio", "IX_Usuario_Premio");
            DropIndex("dbo.Transferencia", new[] { "ContaDestinoId" });
            DropIndex("dbo.Transferencia", new[] { "ContaOrigemId" });
            DropIndex("dbo.Usuario", new[] { "PermissaoId" });
            DropIndex("dbo.Conta", new[] { "UsuarioId" });
            DropTable("dbo.UsuarioPremio");
            DropTable("dbo.Transferencia");
            DropTable("dbo.Premio");
            DropTable("dbo.Permissao");
            DropTable("dbo.Usuario");
            DropTable("dbo.Conta");
        }
    }
}
