using System.Net;
using System.Net.Http.Json;
using Korp.Faturamento.Excecoes;

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
        for (var tentativa = 1; tentativa <= 3; tentativa++)
        {
            try
            {
                var resposta = await _cliente.GetAsync(
                    $"api/produtos/{produtoId}");

                if (resposta.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (resposta.StatusCode ==
                    HttpStatusCode.ServiceUnavailable)
                {
                    if (tentativa < 3)
                    {
                        await Task.Delay(1000);
                        continue;
                    }

                    throw new EstoqueIndisponivelExcecao();
                }

                resposta.EnsureSuccessStatusCode();

                return await resposta.Content
                    .ReadFromJsonAsync<ProdutoEstoqueResposta>();
            }
            catch (HttpRequestException)
            {
                if (tentativa < 3)
                {
                    await Task.Delay(1000);
                    continue;
                }

                throw new EstoqueIndisponivelExcecao();
            }
        }

        throw new EstoqueIndisponivelExcecao();
    }

    public async Task BaixarEstoqueAsync(
        BaixarEstoqueRequisicao requisicao)
    {
        for (var tentativa = 1; tentativa <= 3; tentativa++)
        {
            try
            {
                var resposta = await _cliente.PostAsJsonAsync(
                    "api/estoque/baixar",
                    requisicao);

                if (resposta.StatusCode ==
                    HttpStatusCode.ServiceUnavailable)
                {
                    if (tentativa < 3)
                    {
                        await Task.Delay(1000);
                        continue;
                    }

                    throw new EstoqueIndisponivelExcecao();
                }

                resposta.EnsureSuccessStatusCode();

                return;
            }
            catch (HttpRequestException)
            {
                if (tentativa < 3)
                {
                    await Task.Delay(1000);
                    continue;
                }

                throw new EstoqueIndisponivelExcecao();
            }
        }

        throw new EstoqueIndisponivelExcecao();
    }
}