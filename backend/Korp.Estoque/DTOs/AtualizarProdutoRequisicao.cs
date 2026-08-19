using System.ComponentModel.DataAnnotations;

namespace Korp.Estoque.DTOs;

public class AtualizarProdutoRequisicao
{
    [Required]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    public string Descricao { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Saldo { get; set; }
}