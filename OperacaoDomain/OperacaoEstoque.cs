using ProdutoDomain;
using System.ComponentModel.DataAnnotations;

namespace OperacaoDomain;

public class OperacaoEstoque
{
    
    public OperacaoEstoque(DateTime hora, string motivo, string entradaSaida)
    {
        if (entradaSaida.ToUpper() != "S" && entradaSaida.ToUpper() != "E")
        {
            throw new ArgumentException("Entrada ou Saída só pode ser 'E' ou 'S' ");
        }
        
        Hora = hora;
        Motivo = motivo;
        EntradaSaida = entradaSaida;
    }

    [Key] public Guid Id { get; set; }
    public DateTime Hora { get; set; }
    public string Motivo { get; set; }

    [StringLength(1)]
    public string EntradaSaida { get; set; }

    private List<OperacaoEstoqueDetalhe> _detalhes = [];
    public IReadOnlyList<OperacaoEstoqueDetalhe> Detalhes => _detalhes;

    public void CriarDetalhe(Produto produto, decimal quantidade)
    {
        if (EntradaSaida.ToUpper() == "S" && quantidade < produto.QuantidadeAtual)
        {
            throw new ArgumentException(
                "Não é possível fazer uma saída com quantidade maior do que há no estoque"
            );
        }

        if (!produto.Habilitado)
        {
            throw new ArgumentException(
                "Não é possível fazer operação com produto desabilitado"
            );
        }

        OperacaoEstoqueDetalhe detalhe = new(produto.Id, quantidade);

        _detalhes.Add(detalhe);
    }

}
