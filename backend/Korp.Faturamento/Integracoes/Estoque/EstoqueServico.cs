using System.Net;
using System.Net.Http.Json;

namespace Korp.Faturamento.Integracoes.Estoque;

public class EstoqueServico : IEstoqueServico
{
    private readonly HttpClient _cliente;

    public EstoqueServico(HttpClient cliente)
    {
        _cliente = cliente;
    }

    public async Task<ProdutoEstoqueResposta?> BuscarProdutoAsync(
        int produtoId)
    {
        var resposta = await _cliente.GetAsync(
            $"api/produtos/{produtoId}");

        if (resposta.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        resposta.EnsureSuccessStatusCode();

        return await resposta.Content
            .ReadFromJsonAsync<ProdutoEstoqueResposta>();
    }

    public async Task BaixarEstoqueAsync(
        BaixarEstoqueRequisicao requisicao)
    {
        var resposta = await _cliente.PostAsJsonAsync(
            "api/estoque/baixar",
            requisicao);

        resposta.EnsureSuccessStatusCode();
    }
}