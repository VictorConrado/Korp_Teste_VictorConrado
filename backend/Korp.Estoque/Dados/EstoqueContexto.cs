using Microsoft.EntityFrameworkCore;

namespace Korp.Estoque.Dados;

public class EstoqueContexto : DbContext
{
    public EstoqueContexto(DbContextOptions<EstoqueContexto> opcoes)
        : base(opcoes)
    {
    }
}