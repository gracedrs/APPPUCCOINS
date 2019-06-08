namespace API_PUCCOINS.Migrations
{
    using API_PUCCOINS.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<API_PUCCOINS.Models.API_PUCCOINSContext>
    {
        private readonly bool _pendingMigrations;
        public Configuration()
        {
            var migrator = new DbMigrator(this);
            _pendingMigrations = migrator.GetPendingMigrations().Any();
        }

        protected override void Seed(API_PUCCOINS.Models.API_PUCCOINSContext context)
        {
            if (!_pendingMigrations) return;

            List<Permissao> permissoes = new List<Permissao>
            {
                new Permissao { Descricao = "Admin" },
                new Permissao { Descricao = "Usuario"}
            };

            permissoes.ForEach(s => context.Permissoes.AddOrUpdate(s));
            context.SaveChanges();

            List<Usuario> usuarios = new List<Usuario>
            {
                new Usuario{ Nome = "João da Silva", Senha = "81dc9bdb52d04dc20036dbd8313ed055", Email="joao.silva@puccoins.com.br", Guid = Guid.NewGuid(), PermissaoId = 1 },
                new Usuario{ Nome = "Maria da Silva", Senha = "81dc9bdb52d04dc20036dbd8313ed055", Email="maria.silva@puccoins.com.br", Guid = Guid.NewGuid(), PermissaoId = 2 },
                new Usuario{ Nome = "Jose da Silva", Senha = "81dc9bdb52d04dc20036dbd8313ed055", Email="jose.silva@puccoins.com.br", Guid = Guid.NewGuid(), PermissaoId = 2 }
            };

            usuarios.ForEach(s => context.Usuarios.AddOrUpdate(s));
            context.SaveChanges();

            List<Premio> premios = new List<Premio>
            {
                new Premio{ Descricao = "Camisa", Preco = 15, Imagem = "\\Imagens\\camisa.png", Quantidade = 100 },
                new Premio{ Descricao = "Caneca", Preco = 10, Imagem="\\Imagens\\caneca.png", Quantidade = 100 },
                new Premio{ Descricao = "Boné", Preco = 5, Imagem="\\Imagens\\bone.png", Quantidade = 100 }
            };

            premios.ForEach(s => context.Premios.AddOrUpdate(s));
            context.SaveChanges();

            List<UsuarioPremio> usuarioPremios = new List<UsuarioPremio>
            {
                new UsuarioPremio{ UsuarioId = 1, PremioId = 1, DataSolicitacao = DateTime.Now, DataEntrega = DateTime.Now.AddDays(7) },
                new UsuarioPremio{ UsuarioId = 1, PremioId = 2, DataSolicitacao= DateTime.Now, DataEntrega = DateTime.Now.AddDays(7) },
                new UsuarioPremio{ UsuarioId = 2, PremioId = 3, DataSolicitacao= DateTime.Now, DataEntrega = DateTime.Now.AddDays(7) }
            };

            usuarioPremios.ForEach(s => context.UsuarioPremio.AddOrUpdate(s));
            context.SaveChanges();

            List<Conta> contas = new List<Conta>
            {
                new Conta{ UsuarioId = 1, Saldo = 100 },
                new Conta{ UsuarioId = 2, Saldo = 50 },
                new Conta{ UsuarioId = 3, Saldo = 25 }
            };

            contas.ForEach(s => context.Contas.AddOrUpdate(s));
            context.SaveChanges();

            List<Transferencia> transferencias = new List<Transferencia>
            {
                new Transferencia{ Data = DateTime.Now.AddDays(-7), Valor = 5, ContaOrigemId = 1, ContaDestinoId = 2, Descricao = "Saldar dívida" },
                new Transferencia{ Data = DateTime.Now.AddDays(-6), Valor = 7, ContaOrigemId = 3, ContaDestinoId = 1, Descricao = "Saldar dívida" },
                new Transferencia{ Data = DateTime.Now.AddDays(-5), Valor = 9, ContaOrigemId = 2, ContaDestinoId = 3, Descricao = "Saldar dívida" }
            };

            transferencias.ForEach(s => context.Transferencias.AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}
