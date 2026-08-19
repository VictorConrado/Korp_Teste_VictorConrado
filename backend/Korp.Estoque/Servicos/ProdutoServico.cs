using Korp.Estoque.Dados;
using Korp.Estoque.Dominios;
using Korp.Estoque.DTOs;
using Korp.Estoque.Excecoes;
using Microsoft.EntityFrameworkCore;

namespace Korp.Estoque.Servicos;

public class ProdutoServico : IProdutoServico
{
    private readonly EstoqueContexto _contexto;

    public ProdutoServico(EstoqueContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<ProdutoResposta> CriarAsync(
        CriarProdutoRequisicao requisicao)
    {
        var produtoExistente = await _contexto.Produtos
            .AnyAsync(x => x.Codigo == requisicao.Codigo);

        if (produtoExistente)
            throw new ProdutoDuplicadoExcecao(requisicao.Codigo);

        var produto = new Produto
        {
            Codigo = requisicao.Codigo,
            Descricao = requisicao.Descricao,
            Saldo = requisicao.Saldo
        };

        _contexto.Produtos.Add(produto);

        await _contexto.SaveChangesAsync();

        return ConverterParaResposta(produto);
    }

    public async Task<IEnumerable<ProdutoResposta>> ListarAsync()
    {
        return await _contexto.Produtos
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new ProdutoResposta
            {
                Id = x.Id,
                Codigo = x.Codigo,
                Descricao = x.Descricao,
                Saldo = x.Saldo
            })
            .ToListAsync();
    }

    public async Task<ProdutoResposta> BuscarPorIdAsync(int id)
    {
        var produto = await _contexto.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (produto is null)
            throw new ProdutoNaoEncontradoExcecao(id);

        return ConverterParaResposta(produto);
    }

    public async Task<ProdutoResposta> AtualizarAsync(
        int id,
        AtualizarProdutoRequisicao requisicao)
    {
        var produto = await _contexto.Produtos
            .FirstOrDefaultAsync(x => x.Id == id);

        if (produto is null)
            throw new ProdutoNaoEncontradoExcecao(id);

        var codigoDuplicado = await _contexto.Produtos
            .AnyAsync(x =>
                x.Codigo == requisicao.Codigo &&
                x.Id != id);

        if (codigoDuplicado)
            throw new ProdutoDuplicadoExcecao(requisicao.Codigo);

        produto.Codigo = requisicao.Codigo;
        produto.Descricao = requisicao.Descricao;
        produto.Saldo = requisicao.Saldo;

        await _contexto.SaveChangesAsync();

        return ConverterParaResposta(produto);
    }

    public async Task ExcluirAsync(int id)
    {
        var produto = await _contexto.Produtos
            .FirstOrDefaultAsync(x => x.Id == id);

        if (produto is null)
            throw new ProdutoNaoEncontradoExcecao(id);

        _contexto.Produtos.Remove(produto);

        await _contexto.SaveChangesAsync();
    }

    private static ProdutoResposta ConverterParaResposta(
        Produto produto)
    {
        return new ProdutoResposta
        {
            Id = produto.Id,
            Codigo = produto.Codigo,
            Descricao = produto.Descricao,
            Saldo = produto.Saldo
        };
    }
}