using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GuardiaoDasMemorias.Models;
using GuardiaoDasMemorias.Entities.Cliente;
using GuardiaoDasMemorias.Entities.Musica;
using GuardiaoDasMemorias.Entities.Tema;
using GuardiaoDasMemorias.Entities.Template;
using GuardiaoDasMemorias.Entities.Memoria;
using GuardiaoDasMemorias.Entities.Contrato;
using GuardiaoDasMemorias.Entities.Plano;

namespace GuardiaoDasMemorias.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Clientes> Clientes { get; set; }
    public DbSet<Temas> Temas { get; set; }
    public DbSet<Musicas> Musicas { get; set; }
    public DbSet<Memorias> Memorias { get; set; }
    public DbSet<Templates> Templates { get; set; }
    public DbSet<TipoPagamento> TipoPagamentos { get; set; }
    public DbSet<Planos> Planos { get; set; }
    public DbSet<PlanoLimites> PlanoLimites { get; set; }
    public DbSet<PlanoRecursos> PlanoRecursos { get; set; }
    public DbSet<ContratoStatus> ContratoStatus { get; set; }
    public DbSet<ContratoOrigem> ContratoOrigens { get; set; }
    public DbSet<ContratoMemoria> ContratoMemorias { get; set; }
    public DbSet<ContratoHistorico> ContratoHistoricos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar tabelas do Identity para usar schema 'auth'
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("users", "auth");
        });

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(entity =>
        {
            entity.ToTable("roles", "auth");
        });

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("user_roles", "auth");
        });

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("user_claims", "auth");
        });

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("user_logins", "auth");
        });

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("role_claims", "auth");
        });

        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("user_tokens", "auth");
        });

        // Configuração da entidade Cliente
        modelBuilder.Entity<Clientes>(entity =>
        {
            entity.ToTable("clientes", "cliente");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            // Relacionamento com ApplicationUser
            entity.HasOne(c => c.User)
                .WithOne(u => u.Cliente)
                .HasForeignKey<Clientes>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // Configuração da entidade Tema
        modelBuilder.Entity<Temas>(entity =>
        {
            entity.ToTable("temas", "tema");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(100);
        });

        // Configuração da entidade Musica
        modelBuilder.Entity<Musicas>(entity =>
        {
            entity.ToTable("musicas", "musica");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Caminho)
                .HasColumnName("caminho")
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.MemoriaId)
                .HasColumnName("memoria_id")
                .IsRequired();

            // Relacionamento com Memoria
            entity.HasOne(m => m.Memoria)
                  .WithMany(x => x.Musicas)
                  .HasForeignKey(m => m.MemoriaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da entidade Template
        modelBuilder.Entity<Templates>(entity =>
        {
            entity.ToTable("templates", "template");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Ativo)
                .HasColumnName("ativo")
                .IsRequired()
                .HasDefaultValue(true);
            entity.Property(e => e.TemaId)
                .HasColumnName("tema_id")
                .IsRequired();

            // Relacionamento com Tema (FK)
            entity.HasOne(t => t.Tema)
                .WithMany(te => te.Templates)
                .HasForeignKey(e => e.TemaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da entidade Memoria
        modelBuilder.Entity<Entities.Memoria.Memorias>(entity =>
        {
            entity.ToTable("memorias", "memoria");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.TemaId)
                .HasColumnName("tema_id")
                .IsRequired();
            entity.Property(e => e.TemplateId)
                .HasColumnName("template_id")
                .IsRequired();
            entity.Property(e => e.ClienteId)
                .HasColumnName("cliente_id")
                .IsRequired();
            entity.Property(e => e.MemoriaHash)
                .HasColumnName("memoria_hash")
                .IsRequired();

            entity.HasIndex(e => e.MemoriaHash).IsUnique();

            // Relacionamento com Tema
            entity.HasOne(t => t.Tema)
                .WithMany(m => m.Memorias)
                .HasForeignKey(e => e.TemaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Cliente
            entity.HasOne(m => m.Cliente)
               .WithMany(c => c.Memorias)
               .HasForeignKey(m => m.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Template
            entity.HasOne(t => t.Template)
                .WithMany(m => m.Memorias)
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da entidade TipoPagamento
        modelBuilder.Entity<TipoPagamento>(entity =>
        {
            entity.ToTable("tipo_pagamento", "plano");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(50);

            // Seed data - valores padrão
            entity.HasData(
                new TipoPagamento { Id = 1, Nome = "Pagamento Único" },
                new TipoPagamento { Id = 2, Nome = "Assinatura" }
            );
        });

        // Configuração da entidade Planos
        modelBuilder.Entity<Planos>(entity =>
        {
            entity.ToTable("planos", "plano");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.TemaId)
                .HasColumnName("tema_id")
                .IsRequired();
            entity.Property(e => e.TipoPagamentoId)
                .HasColumnName("tipo_pagamento_id")
                .IsRequired();
            entity.Property(e => e.Code)
                .HasColumnName("code")
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(500);
            entity.Property(e => e.Preco)
                .HasColumnName("preco")
                .IsRequired()
                .HasPrecision(10, 2); // Precisão para valores monetários
            entity.Property(e => e.Ativo)
                .HasColumnName("ativo")
                .IsRequired()
                .HasDefaultValue(true);
            entity.Property(e => e.Ordem)
                .HasColumnName("ordem")
                .IsRequired();
            entity.Property(e => e.Criado)
                .HasColumnName("criado")
                .IsRequired();
            entity.Property(e => e.Atualizado)
                .HasColumnName("atualizado");

            // Relacionamento com Tema
            entity.HasOne(p => p.Tema)
                .WithMany(t => t.Planos)
                .HasForeignKey(p => p.TemaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com TipoPagamento
            entity.HasOne(p => p.TipoPagamento)
                .WithMany(tp => tp.Planos)
                .HasForeignKey(p => p.TipoPagamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para melhor performance
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => new { e.TemaId, e.Ativo });
        });

        // Configuração da entidade PlanoLimites
        modelBuilder.Entity<PlanoLimites>(entity =>
        {
            entity.ToTable("plano_limites", "plano");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.PlanoId)
                .HasColumnName("plano_id")
                .IsRequired();
            entity.Property(e => e.Propriedade)
                .HasColumnName("propriedade")
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Valor)
                .HasColumnName("valor")
                .IsRequired();
            entity.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(500);

            // Relacionamento com Plano
            entity.HasOne(pl => pl.Plano)
                .WithMany(p => p.PlanoLimites)
                .HasForeignKey(pl => pl.PlanoId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade: se deletar o plano, deleta os limites

            // Índice composto para evitar duplicação de propriedades por plano
            entity.HasIndex(e => new { e.PlanoId, e.Propriedade }).IsUnique();
        });

        // Configuração da entidade PlanoRecursos
        modelBuilder.Entity<PlanoRecursos>(entity =>
        {
            entity.ToTable("plano_recursos", "plano");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.PlanoId)
                .HasColumnName("plano_id")
                .IsRequired();
            entity.Property(e => e.RecursoKey)
                .HasColumnName("recurso_key")
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.Ativo)
                .HasColumnName("ativo")
                .IsRequired()
                .HasDefaultValue(true);
            entity.Property(e => e.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            // Relacionamento com Plano
            entity.HasOne(pr => pr.Plano)
                .WithMany(p => p.PlanoRecursos)
                .HasForeignKey(pr => pr.PlanoId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade: se deletar o plano, deleta os recursos

            // Índices para buscar recursos por plano e evitar duplicação
            entity.HasIndex(e => new { e.PlanoId, e.RecursoKey }).IsUnique();
            entity.HasIndex(e => new { e.PlanoId, e.Ativo, e.Ordem });
        });

        // Configuração da entidade ContratoStatus
        modelBuilder.Entity<ContratoStatus>(entity =>
        {
            entity.ToTable("contrato_status", "contrato");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(50);

            // Seed data - valores padrão
            entity.HasData(
                new ContratoStatus { Id = 1, Nome = "Pendente" },
                new ContratoStatus { Id = 2, Nome = "Ativo" },
                new ContratoStatus { Id = 3, Nome = "Cancelado" },
                new ContratoStatus { Id = 4, Nome = "Expirado" }
            );
        });

        // Configuração da entidade ContratoOrigem
        modelBuilder.Entity<ContratoOrigem>(entity =>
        {
            entity.ToTable("contrato_origem", "contrato");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(50);

            // Seed data - valores padrão
            entity.HasData(
                new ContratoOrigem { Id = 1, Nome = "Compra no Site" },
                new ContratoOrigem { Id = 2, Nome = "Afiliado" },
                new ContratoOrigem { Id = 3, Nome = "Presente Admin" }
            );
        });

        // Configuração da entidade ContratoMemoria
        modelBuilder.Entity<ContratoMemoria>(entity =>
        {
            entity.ToTable("contrato_memoria", "contrato");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.MemoriaId)
                .HasColumnName("memoria_id")
                .IsRequired();
            entity.Property(e => e.PlanoId)
                .HasColumnName("plano_id")
                .IsRequired();
            entity.Property(e => e.ContratoStatusId)
                .HasColumnName("contrato_status_id")
                .IsRequired();
            entity.Property(e => e.ContratoOrigemId)
                .HasColumnName("contrato_origem_id")
                .IsRequired();
            entity.Property(e => e.ClienteId)
                .HasColumnName("cliente_id")
                .IsRequired();
            entity.Property(e => e.ValorPago)
                .HasColumnName("valor_pago")
                .IsRequired()
                .HasPrecision(10, 2);
            entity.Property(e => e.TransacaoId)
                .HasColumnName("transacao_id")
                .HasMaxLength(200);
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .IsRequired();
            entity.Property(e => e.PagoEm)
                .HasColumnName("pago_em");
            entity.Property(e => e.ExpiraEm)
                .HasColumnName("expira_em");
            entity.Property(e => e.CanceladoEm)
                .HasColumnName("cancelado_em");

            // Relacionamento com Memoria
            entity.HasOne(cm => cm.Memoria)
                .WithMany(m => m.Contratos)
                .HasForeignKey(cm => cm.MemoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Plano
            entity.HasOne(cm => cm.Plano)
                .WithMany(p => p.Contratos)
                .HasForeignKey(cm => cm.PlanoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com ContratoStatus
            entity.HasOne(cm => cm.ContratoStatus)
                .WithMany(cs => cs.ContratoMemorias)
                .HasForeignKey(cm => cm.ContratoStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com ContratoOrigem
            entity.HasOne(cm => cm.ContratoOrigem)
                .WithMany(co => co.ContratoMemorias)
                .HasForeignKey(cm => cm.ContratoOrigemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Cliente (comprador)
            entity.HasOne(cm => cm.Cliente)
                .WithMany(c => c.Contratos)
                .HasForeignKey(cm => cm.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para melhor performance
            entity.HasIndex(e => e.MemoriaId);
            entity.HasIndex(e => e.ClienteId);
            entity.HasIndex(e => new { e.ContratoStatusId, e.ExpiraEm });
        });

        // Configuração da entidade ContratoHistorico
        modelBuilder.Entity<ContratoHistorico>(entity =>
        {
            entity.ToTable("contrato_historico", "contrato");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.ContratoAntigoId)
                .HasColumnName("contrato_antigo_id")
                .IsRequired();
            entity.Property(e => e.ContratoNovoId)
                .HasColumnName("contrato_novo_id")
                .IsRequired();
            entity.Property(e => e.TipoMudanca)
                .HasColumnName("tipo_mudanca")
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Observacao)
                .HasColumnName("observacao")
                .HasMaxLength(1000);
            entity.Property(e => e.RealizadoEm)
                .HasColumnName("realizado_em")
                .IsRequired();

            // Relacionamento com ContratoMemoria (Antigo)
            entity.HasOne(ch => ch.ContratoAntigo)
                .WithMany(cm => cm.HistoricoComoAntigo)
                .HasForeignKey(ch => ch.ContratoAntigoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com ContratoMemoria (Novo)
            entity.HasOne(ch => ch.ContratoNovo)
                .WithMany(cm => cm.HistoricoComoNovo)
                .HasForeignKey(ch => ch.ContratoNovoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para rastreamento e performance
            entity.HasIndex(e => e.ContratoAntigoId);
            entity.HasIndex(e => e.ContratoNovoId);
            entity.HasIndex(e => new { e.TipoMudanca, e.RealizadoEm });
            
            // Garantir que um contrato não seja antigo e novo ao mesmo tempo
            entity.HasCheckConstraint(
                "CK_ContratoHistorico_DiferentesContratos",
                "contrato_antigo_id != contrato_novo_id"
            );
        });
    }
}
