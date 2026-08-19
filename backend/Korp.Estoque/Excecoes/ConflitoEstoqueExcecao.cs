namespace Korp.Estoque.Excecoes;

public class ConflitoEstoqueExcecao : Exception
{
    public ConflitoEstoqueExcecao()
        : base(
            "O estoque foi alterado por outra operação. " +
            "Tente novamente.")
    {
    }
}