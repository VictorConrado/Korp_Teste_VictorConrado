using Korp.Faturamento.Dominios;

namespace Korp.Faturamento.DTOs;

public class NotaFiscalResposta
{
    public int Id { get; set; }

    public string Numero { get; set; } = string.Empty;

    public DateTime DataEmissao { get; set; }

    public StatusNotaFiscal Status { get; set; }

    public decimal ValorTotal { get; set; }

    public List<ItemNotaFiscalResposta> Itens { get; set; } = [];
}

public class ItemNotaFiscalResposta
{
    public int ProdutoId { get; set; }

    public string CodigoProduto { get; set; } = string.Empty;

    public string DescricaoProduto { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public decimal ValorUnitario { get; set; }

    public decimal ValorTotal { get; set; }
}