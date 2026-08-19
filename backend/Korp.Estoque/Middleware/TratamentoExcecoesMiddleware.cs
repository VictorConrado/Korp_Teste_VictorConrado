using System.Net;
using System.Text.Json;
using Korp.Estoque.Excecoes;
using Microsoft.EntityFrameworkCore;

namespace Korp.Estoque.Middleware;

public class TratamentoExcecoesMiddleware
{
    private readonly RequestDelegate _proximo;

    public TratamentoExcecoesMiddleware(RequestDelegate proximo)
    {
        _proximo = proximo;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _proximo(contexto);
        }
        catch (Exception excecao)
        {
            await TratarExcecaoAsync(contexto, excecao);
        }
    }

    private static async Task TratarExcecaoAsync(
        HttpContext contexto,
        Exception excecao)
    {
        var status = HttpStatusCode.InternalServerError;
        var mensagem = "Ocorreu um erro interno no servidor.";

        switch (excecao)
        {
            case ProdutoNaoEncontradoExcecao:
                status = HttpStatusCode.NotFound;
                mensagem = excecao.Message;
                break;

            case ProdutoDuplicadoExcecao:
                status = HttpStatusCode.Conflict;
                mensagem = excecao.Message;
                break;

            case EstoqueInsuficienteExcecao:
                status = HttpStatusCode.BadRequest;
                mensagem = excecao.Message;
                break;

            case ConflitoEstoqueExcecao:
                status = HttpStatusCode.Conflict;
                mensagem = excecao.Message;
                break;

            case DbUpdateConcurrencyException:
                status = HttpStatusCode.Conflict;
                mensagem =
                    "O estoque foi alterado por outra operação. " +
                    "Tente novamente.";
                break;
        }

        contexto.Response.StatusCode = (int)status;
        contexto.Response.ContentType = "application/json";

        var resposta = new
        {
            mensagem
        };

        await contexto.Response.WriteAsync(
            JsonSerializer.Serialize(resposta));
    }
}