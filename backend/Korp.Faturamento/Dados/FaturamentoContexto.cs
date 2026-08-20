using Korp.Faturamento.Dominios;
using Microsoft.EntityFrameworkCore;

namespace Korp.Faturamento.Dados;

public class FaturamentoContexto : DbContext
{
    public FaturamentoContexto(
        DbContextOptions<FaturamentoContexto> opcoes)
        : base(opcoes)
    {
    }

    public DbSet<NotaFiscal> NotasFiscais => Set<NotaFiscal>();

    public DbSet<ItemNotaFiscal> ItensNotaFiscal =>
        Set<ItemNotaFiscal>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.Entity<NotaFiscal>(nota =>
        {
            nota.ToTable("NotasFiscais");

            nota.HasKey(x => x.Id);

            nota.Property(x => x.Numero)
                .IsRequired()
                .HasMaxLength(50);

            nota.HasIndex(x => x.Numero)
                .IsUnique();

            nota.Property(x => x.DataEmissao)
                .IsRequired();

            nota.Property(x => x.Status)
                .IsRequired();

            nota.Property(x => x.ValorTotal)
                .HasPrecision(18, 2);

            nota.HasMany(x => x.Itens)
                .WithOne(x => x.NotaFiscal)
                .HasForeignKey(x => x.NotaFiscalId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelo.Entity<ItemNotaFiscal>(item =>
        {
            item.ToTable("ItensNotaFiscal");

            item.HasKey(x => x.Id);

            item.Property(x => x.CodigoProduto)
                .IsRequired()
                .HasMaxLength(50);

            item.Property(x => x.DescricaoProduto)
                .IsRequired()
                .HasMaxLength(200);

            item.Property(x => x.Quantidade)
                .IsRequired();

            item.Property(x => x.ValorUnitario)
                .HasPrecision(18, 2);

            item.Property(x => x.ValorTotal)
                .HasPrecision(18, 2);
        });
    }
}