namespace Korp.Estoque.Excecoes;

public class EstoqueInsuficienteExcecao : Exception
{
    public EstoqueInsuficienteExcecao(
        int produtoId,
        int saldoAtual,
        int quantidadeSolicitada)
        : base(
            $"Estoque insuficiente para o produto {produtoId}. " +
            $"Saldo atual: {saldoAtual}. " +
            $"Quantidade solicitada: {quantidadeSolicitada}.")
    {
    }
}