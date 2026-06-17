namespace ApiControleEstoque.Contracts;

public record PostOperacaoRequest
{
    public required string Motivo { get; set; }
    public required string EntradaSaida { get; set; }
    public required List<PostOperacaoDetalheRequest> Detalhes { get; set; }
}

public record PostOperacaoDetalheRequest
{
    public required Guid ProdutoId { get; set; }
    public required decimal Quantidade { get; set; }
}
