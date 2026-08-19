using Korp.Estoque.DTOs;
using Korp.Estoque.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Korp.Estoque.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoServico _produtoServico;

    public ProdutosController(IProdutoServico produtoServico)
    {
        _produtoServico = produtoServico;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var produtos = await _produtoServico.ListarAsync();

        return Ok(produtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var produto = await _produtoServico.BuscarPorIdAsync(id);

        return Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        CriarProdutoRequisicao requisicao)
    {
        var produto = await _produtoServico.CriarAsync(requisicao);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = produto.Id },
            produto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(
        int id,
        AtualizarProdutoRequisicao requisicao)
    {
        var produto = await _produtoServico.AtualizarAsync(
            id,
            requisicao);

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        await _produtoServico.ExcluirAsync(id);

        return NoContent();
    }
}