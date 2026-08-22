namespace Korp.Faturamento.Excecoes;

public class EstoqueIndisponivelExcecao : Exception
{
    public EstoqueIndisponivelExcecao()
        : base("O serviço de estoque está temporariamente indisponível.")
    {
    }
}