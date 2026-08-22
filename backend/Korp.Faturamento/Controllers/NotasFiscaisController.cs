using Korp.Faturamento.DTOs;
using Korp.Faturamento.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Korp.Faturamento.Controllers;

[ApiController]
[Route("api/notas-fiscais")]
public class NotasFiscaisController : ControllerBase
{
    private readonly INotaFiscalServico _notaFiscalServico;

    public NotasFiscaisController(
        INotaFiscalServico notaFiscalServico)
    {
        _notaFiscalServico = notaFiscalServico;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        CriarNotaFiscalRequisicao requisicao)
    {
        var resposta =
            await _notaFiscalServico.CriarAsync(requisicao);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = resposta.Id },
            resposta);
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var respostas =
            await _notaFiscalServico.ListarAsync();

        return Ok(respostas);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var resposta =
            await _notaFiscalServico.BuscarPorIdAsync(id);

        return Ok(resposta);
    }

    [HttpPost("{id:int}/imprimir")]
    public async Task<IActionResult> Imprimir(int id)
    {
        var resposta =
            await _notaFiscalServico.ImprimirAsync(id);

        return Ok(resposta);
    }
}