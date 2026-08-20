namespace Korp.Estoque.Dominios;

public class OperacaoIdempotencia
{
    public int Id { get; set; }

    public string Chave { get; set; } = string.Empty;

    public string Operacao { get; set; } = string.Empty;

    public DateTime CriadaEm { get; set; }
}