using System.ComponentModel.DataAnnotations;

namespace ProdutoDomain;

public class UnidadeMedida
{
    public UnidadeMedida(string sigla, string? descricao, bool fracionavel)
    {
        Sigla = sigla;
        Descricao = descricao;
        Fracionavel = fracionavel;
    }

    [Key]
    public Guid Id { get; set; }

    [StringLength(5)]
    public required string Sigla { get; set; }

    public string? Descricao { get; set; }

    public bool Fracionavel { get; set; } = false;
}
