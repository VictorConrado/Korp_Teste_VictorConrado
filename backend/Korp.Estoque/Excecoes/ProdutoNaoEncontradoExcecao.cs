namespace Korp.Estoque.Excecoes;

public class ProdutoNaoEncontradoExcecao : Exception
{
    public ProdutoNaoEncontradoExcecao(int id)
        : base($"Produto com ID {id} não foi encontrado.")
    {
    }
}