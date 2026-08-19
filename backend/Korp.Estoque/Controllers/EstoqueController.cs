using Korp.Estoque.DTOs;
using Korp.Estoque.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Korp.Estoque.Controllers;

[ApiController]
[Route("api/estoque")]
public class EstoqueController : ControllerBase
{
    private readonly IProdutoServico _produtoServico;

    public EstoqueController(IProdutoServico produtoServico)
    {
        _produtoServico = produtoServico;
    }

    [HttpPost("baixar")]
    public async Task<IActionResult> Baixar(
        BaixarEstoqueRequisicao requisicao)
    {
        await _produtoServico.BaixarEstoqueAsync(
            requisicao);

        return Ok(new
        {
            mensagem = "Estoque atualizado com sucesso."
        });
    }
}