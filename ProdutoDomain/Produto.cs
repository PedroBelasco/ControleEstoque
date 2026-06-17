using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdutoDomain;

public class Produto
{
    public Produto(
        bool habilitado, Guid categoriaId, Guid unidadeMedidaId,
        string nome, string? descricao, decimal quantidadeAtual)
    {
        Habilitado = habilitado;
        CategoriaId = categoriaId;
        UnidadeMedidaId = unidadeMedidaId;
        Nome = nome;
        Descricao = descricao;
        QuantidadeAtual = quantidadeAtual;
    }

    [Key] public Guid Id { get; set; }
    public bool Habilitado { get; set; }
    public Guid CategoriaId { get; set; }
    public CategoriaProduto? Categoria { get; set; }
    public Guid UnidadeMedidaId { get; set; }
    public UnidadeMedida? UnidadeMedida { get; set; }
    [StringLength(50)] public string Nome { get; set; }
    public string? NomeArquivoFoto { get; set; }
    public string? Descricao { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal QuantidadeAtual { get; set; }
}
