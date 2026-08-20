namespace Korp.Faturamento.Integracoes.Estoque;

public interface IEstoqueServico
{
    Task<ProdutoEstoqueResposta?> BuscarProdutoAsync(
        int produtoId);

    Task BaixarEstoqueAsync(
        BaixarEstoqueRequisicao requisicao);
}