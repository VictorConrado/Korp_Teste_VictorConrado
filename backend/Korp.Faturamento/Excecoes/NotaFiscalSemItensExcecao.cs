namespace Korp.Faturamento.Excecoes;

public class NotaFiscalSemItensExcecao : Exception
{
    public NotaFiscalSemItensExcecao()
        : base("A nota fiscal deve possuir pelo menos um item.")
    {
    }
}