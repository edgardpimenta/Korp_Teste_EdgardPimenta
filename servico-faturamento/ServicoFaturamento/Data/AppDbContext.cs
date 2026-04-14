using Microsoft.EntityFrameworkCore;
using ServicoFaturamento.Models;

namespace ServicoFaturamento.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<NotaFiscal> NotasFiscais { get; set; }
    public DbSet<ItemNota> ItensNota { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotaFiscal>(entity =>
        {
            entity.ToTable("notas_fiscais");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Numeracao).HasColumnName("numeracao");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(e => e.Numeracao).IsUnique();
        });

        modelBuilder.Entity<ItemNota>(entity =>
        {
            entity.ToTable("itens_nota");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NotaFiscalId).HasColumnName("nota_fiscal_id");
            entity.Property(e => e.ProdutoId).HasColumnName("produto_id");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            entity.HasOne(e => e.NotaFiscal)
                  .WithMany(n => n.Itens)
                  .HasForeignKey(e => e.NotaFiscalId);
        });
    }
}