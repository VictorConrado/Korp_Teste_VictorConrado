using System.Net;
using System.Text.Json;
using Korp.Faturamento.Excecoes;

namespace Korp.Faturamento.Middleware;

public class TratamentoExcecoesMiddleware
{
    private readonly RequestDelegate _proximo;

    public TratamentoExcecoesMiddleware(
        RequestDelegate proximo)
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
            await TratarAsync(contexto, excecao);
        }
    }

    private static async Task TratarAsync(
        HttpContext contexto,
        Exception excecao)
    {
        var status = HttpStatusCode.InternalServerError;
        var mensagem = "Ocorreu um erro interno no servidor.";

        switch (excecao)
        {
            case NotaFiscalNaoEncontradaExcecao:
                status = HttpStatusCode.NotFound;
                mensagem = excecao.Message;
                break;

            case NotaFiscalJaImpressaExcecao:
                status = HttpStatusCode.Conflict;
                mensagem = excecao.Message;
                break;

            case NotaFiscalSemItensExcecao:
                status = HttpStatusCode.BadRequest;
                mensagem = excecao.Message;
                break;

            case ArgumentException:
                status = HttpStatusCode.BadRequest;
                mensagem = excecao.Message;
                break;

            case HttpRequestException:
                status = HttpStatusCode.BadGateway;
                mensagem =
                    "Não foi possível comunicar com o serviço de estoque.";
                break;
        }

        contexto.Response.StatusCode = (int)status;
        contexto.Response.ContentType = "application/json";

        await contexto.Response.WriteAsync(
            JsonSerializer.Serialize(new
            {
                mensagem
            }));
    }
}