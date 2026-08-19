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
    }
}