namespace Korp.Faturamento.Excecoes;

public class NotaFiscalNaoEncontradaExcecao : Exception
{
    public NotaFiscalNaoEncontradaExcecao(int id)
        : base($"Nota fiscal com ID {id} não foi encontrada.")
    {
    }
}