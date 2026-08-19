namespace Korp.Estoque.Excecoes;

public class ProdutoDuplicadoExcecao : Exception
{
    public ProdutoDuplicadoExcecao(string codigo)
        : base($"Já existe um produto cadastrado com o código '{codigo}'.")
    {
    }
}