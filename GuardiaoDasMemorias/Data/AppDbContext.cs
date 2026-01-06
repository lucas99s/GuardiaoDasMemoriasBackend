using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GuardiaoDasMemorias.Models;
using Entities = GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Entities.Cliente> Clientes { get; set; }
    public DbSet<Entities.Tema> Temas { get; set; }
    public DbSet<Entities.Musica> Musicas { get; set; }
    public DbSet<Entities.Memoria> Memorias { get; set; }
    public DbSet<Entities.Template> Templates { get; set; }

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
        modelBuilder.Entity<Entities.Cliente>(entity =>
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
            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(200);
            entity.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .IsRequired()
                .HasMaxLength(20);
        });

        // Configuração da entidade Tema
        modelBuilder.Entity<Entities.Tema>(entity =>
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
        modelBuilder.Entity<Entities.Musica>(entity =>
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
            entity.HasOne<Entities.Memoria>()
                .WithMany()
                .HasForeignKey(e => e.MemoriaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade Template
        modelBuilder.Entity<Entities.Template>(entity =>
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
            entity.HasOne<Entities.Tema>()
                .WithMany()
                .HasForeignKey(e => e.TemaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da entidade Memoria
        modelBuilder.Entity<Entities.Memoria>(entity =>
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

            // Relacionamento com Tema
            entity.HasOne<Entities.Tema>()
                .WithMany()
                .HasForeignKey(e => e.TemaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Cliente
            entity.HasOne<Entities.Cliente>()
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Template
            entity.HasOne<Entities.Template>()
                .WithMany()
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
