using Korp.Estoque.Dominios;
using Microsoft.EntityFrameworkCore;

namespace Korp.Estoque.Dados;

public class EstoqueContexto : DbContext
{
    public EstoqueContexto(DbContextOptions<EstoqueContexto> opcoes)
        : base(opcoes)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();

    public DbSet<OperacaoIdempotencia> OperacoesIdempotencia => Set<OperacaoIdempotencia>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.Entity<Produto>(produto =>
        {
            produto.ToTable("Produtos");

            produto.HasKey(x => x.Id);

            produto.Property(x => x.Codigo)
                .IsRequired()
                .HasMaxLength(50);

            produto.HasIndex(x => x.Codigo)
                .IsUnique();

            produto.Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(200);

            produto.Property(x => x.Saldo)
                .IsRequired();

            produto.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelo.Entity<OperacaoIdempotencia>(operacao =>
        {
            operacao.ToTable("OperacoesIdempotencia");

            operacao.HasKey(x => x.Id);

            operacao.Property(x => x.Chave)
                .IsRequired()
                .HasMaxLength(100);

            operacao.Property(x => x.Operacao)
                .IsRequired()
                .HasMaxLength(100);

            operacao.Property(x => x.CriadaEm)
                .IsRequired();

            operacao.HasIndex(x => new
            {
                x.Chave,
                x.Operacao
            })
            .IsUnique();
        });
    }
}