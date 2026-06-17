using ProdutoDomain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperacaoDomain;

public class OperacaoEstoqueDetalhe
{
    internal OperacaoEstoqueDetalhe(Guid produtoId, decimal quantidade)
    {
        if (produtoId == Guid.Empty)
        {
            throw new ArgumentException("O produto deve ser obrigatoriamente informado");
        }

        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade deve ser maior do que zero");
        }

        ProdutoId = produtoId;
        Quantidade = quantidade;
    }

    [Key] public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public Produto? Produto { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Quantidade { get; set; }
}
