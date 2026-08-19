namespace Korp.Estoque.DTOs;

public class BaixarEstoqueRequisicao
{
    public int ProdutoId { get; set; }

    public int Quantidade { get; set; }

    public string ChaveIdempotencia { get; set; } = string.Empty;
}