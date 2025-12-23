using Microsoft.EntityFrameworkCore;
using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Tema> Temas { get; set; }
    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Memoria> Memorias { get; set; }
    public DbSet<Template> Templates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Cliente
        modelBuilder.Entity<Cliente>(entity =>
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
        modelBuilder.Entity<Tema>(entity =>
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
        modelBuilder.Entity<Musica>(entity =>
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
            entity.Property(e => e.ClienteId)
                .HasColumnName("cliente_id")
                .IsRequired();

            // Relacionamento com Cliente
            entity.HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade Template
        modelBuilder.Entity<Template>(entity =>
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
            entity.HasOne<Tema>()
                .WithMany()
                .HasForeignKey(e => e.TemaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da entidade Memoria
        modelBuilder.Entity<Memoria>(entity =>
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

            // Relacionamento com Tema
            entity.HasOne<Tema>()
                .WithMany()
                .HasForeignKey(e => e.TemaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Cliente
            entity.HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Template
            entity.HasOne<Template>()
                .WithMany()
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
