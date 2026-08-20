namespace Korp.Faturamento.Excecoes;

public class NotaFiscalJaImpressaExcecao : Exception
{
    public NotaFiscalJaImpressaExcecao(int id)
        : base($"A nota fiscal com ID {id} já foi impressa.")
    {
    }
}