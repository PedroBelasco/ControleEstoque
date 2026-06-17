using System.ComponentModel.DataAnnotations;

namespace ProdutoDomain;

public class CategoriaProduto
{
    public CategoriaProduto(string nome, string? descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }

    [Key] public Guid Id { get; set; }
    [StringLength(100)] public required string Nome { get; set; }
    public string? Descricao { get; set; }
}
