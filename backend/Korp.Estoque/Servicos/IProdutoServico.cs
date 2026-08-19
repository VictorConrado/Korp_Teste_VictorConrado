using Korp.Estoque.DTOs;

namespace Korp.Estoque.Servicos;

public interface IProdutoServico
{
    Task<ProdutoResposta> CriarAsync(
        CriarProdutoRequisicao requisicao);

    Task<IEnumerable<ProdutoResposta>> ListarAsync();

    Task<ProdutoResposta> BuscarPorIdAsync(int id);

    Task<ProdutoResposta> AtualizarAsync(
        int id,
        AtualizarProdutoRequisicao requisicao);

    Task ExcluirAsync(int id);
}