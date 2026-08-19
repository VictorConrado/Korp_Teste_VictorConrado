namespace Korp.Estoque.Dominios;

public class Produto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public int Saldo { get; set; }

    public byte[] RowVersion { get; set; } = [];
}