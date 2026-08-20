namespace Korp.Faturamento.Dominios;

public class NotaFiscal
{
    public int Id { get; set; }

    public string Numero { get; set; } = string.Empty;

    public DateTime DataEmissao { get; set; }

    public StatusNotaFiscal Status { get; set; }

    public decimal ValorTotal { get; set; }

    public List<ItemNotaFiscal> Itens { get; set; } = [];
}