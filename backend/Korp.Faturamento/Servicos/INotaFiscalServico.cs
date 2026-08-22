using Korp.Faturamento.DTOs;

namespace Korp.Faturamento.Servicos;

public interface INotaFiscalServico
{
    Task<NotaFiscalResposta> CriarAsync(
        CriarNotaFiscalRequisicao requisicao);

    Task<List<NotaFiscalResposta>> ListarAsync();
    
    Task<NotaFiscalResposta> BuscarPorIdAsync(int id);

    Task<NotaFiscalResposta> ImprimirAsync(int id);
}