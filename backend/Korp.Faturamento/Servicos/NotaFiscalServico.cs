using Korp.Faturamento.Dados;
using Korp.Faturamento.Dominios;
using Korp.Faturamento.DTOs;
using Korp.Faturamento.Excecoes;
using Korp.Faturamento.Integracoes.Estoque;
using Microsoft.EntityFrameworkCore;

namespace Korp.Faturamento.Servicos;

public class NotaFiscalServico : INotaFiscalServico
{
    private readonly FaturamentoContexto _contexto;
    private readonly IEstoqueServico _estoqueServico;

    public NotaFiscalServico(
        FaturamentoContexto contexto,
        IEstoqueServico estoqueServico)
    {
        _contexto = contexto;
        _estoqueServico = estoqueServico;
    }

    public async Task<NotaFiscalResposta> CriarAsync(
        CriarNotaFiscalRequisicao requisicao)
    {
        if (requisicao.Itens is null ||
            requisicao.Itens.Count == 0)
        {
            throw new NotaFiscalSemItensExcecao();
        }

        var nota = new NotaFiscal
        {
            Numero = GerarNumero(),
            DataEmissao = DateTime.UtcNow,
            Status = StatusNotaFiscal.Rascunho
        };

        foreach (var itemRequisicao in requisicao.Itens)
        {
            if (itemRequisicao.Quantidade <= 0)
            {
                throw new ArgumentException(
                    "A quantidade do item deve ser maior que zero.");
            }

            if (itemRequisicao.ValorUnitario < 0)
            {
                throw new ArgumentException(
                    "O valor unitário não pode ser negativo.");
            }

            var produto =
                await _estoqueServico.BuscarProdutoAsync(
                    itemRequisicao.ProdutoId);

            if (produto is null)
            {
                throw new ArgumentException(
                    $"Produto com ID {itemRequisicao.ProdutoId} " +
                    "não foi encontrado no estoque.");
            }

            var item = new ItemNotaFiscal
            {
                ProdutoId = produto.Id,
                CodigoProduto = produto.Codigo,
                DescricaoProduto = produto.Descricao,
                Quantidade = itemRequisicao.Quantidade,
                ValorUnitario = itemRequisicao.ValorUnitario,
                ValorTotal =
                    itemRequisicao.Quantidade *
                    itemRequisicao.ValorUnitario
            };

            nota.Itens.Add(item);
        }

        nota.ValorTotal = nota.Itens.Sum(x => x.ValorTotal);

        _contexto.NotasFiscais.Add(nota);

        await _contexto.SaveChangesAsync();

        return Mapear(nota);
    }

    public async Task<NotaFiscalResposta> BuscarPorIdAsync(
        int id)
    {
        var nota = await _contexto.NotasFiscais
            .Include(x => x.Itens)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (nota is null)
        {
            throw new NotaFiscalNaoEncontradaExcecao(id);
        }

        return Mapear(nota);
    }

    public async Task<NotaFiscalResposta> ImprimirAsync(
        int id)
    {
        var nota = await _contexto.NotasFiscais
            .Include(x => x.Itens)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (nota is null)
        {
            throw new NotaFiscalNaoEncontradaExcecao(id);
        }

        if (nota.Status == StatusNotaFiscal.Impressa)
        {
            throw new NotaFiscalJaImpressaExcecao(id);
        }

        foreach (var item in nota.Itens)
        {
            var produto =
                await _estoqueServico.BuscarProdutoAsync(
                    item.ProdutoId);

            if (produto is null)
            {
                throw new ArgumentException(
                    $"Produto com ID {item.ProdutoId} " +
                    "não foi encontrado no estoque.");
            }

            if (produto.Saldo < item.Quantidade)
            {
                throw new ArgumentException(
                    $"Estoque insuficiente para o produto " +
                    $"{item.ProdutoId}. " +
                    $"Saldo atual: {produto.Saldo}. " +
                    $"Quantidade solicitada: {item.Quantidade}.");
            }
        }

        foreach (var item in nota.Itens)
        {
            await _estoqueServico.BaixarEstoqueAsync(
                new BaixarEstoqueRequisicao
                {
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    ChaveIdempotencia =
                        $"NF-{nota.Id}-PRODUTO-{item.ProdutoId}"
                });
        }

        nota.Status = StatusNotaFiscal.Impressa;

        await _contexto.SaveChangesAsync();

        return Mapear(nota);
    }

    private static string GerarNumero()
    {
        return $"NF-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }

    private static NotaFiscalResposta Mapear(
        NotaFiscal nota)
    {
        return new NotaFiscalResposta
        {
            Id = nota.Id,
            Numero = nota.Numero,
            DataEmissao = nota.DataEmissao,
            Status = nota.Status,
            ValorTotal = nota.ValorTotal,
            Itens = nota.Itens
                .Select(item => new ItemNotaFiscalResposta
                {
                    ProdutoId = item.ProdutoId,
                    CodigoProduto = item.CodigoProduto,
                    DescricaoProduto = item.DescricaoProduto,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorTotal = item.ValorTotal
                })
                .ToList()
        };
    }
}