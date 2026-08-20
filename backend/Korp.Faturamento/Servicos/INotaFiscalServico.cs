using Korp.Faturamento.DTOs;

namespace Korp.Faturamento.Servicos;

public interface INotaFiscalServico
{
    Task<NotaFiscalResposta> CriarAsync(
        CriarNotaFiscalRequisicao requisicao);

    Task<NotaFiscalResposta> BuscarPorIdAsync(int id);

    Task<NotaFiscalResposta> ImprimirAsync(int id);
}