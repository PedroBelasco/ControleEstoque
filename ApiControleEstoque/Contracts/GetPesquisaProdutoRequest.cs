namespace ApiControleEstoque.Contracts;

public record GetPesquisaProdutoRequest
{
    public required Guid Id { get; set; }
    public required string CategoriaNome { get; set; }
    public required string ProdutoNome { get; set; }
    public required decimal QuantidadeAtual { get; set; }
    public required string UnidadeMedida { get; set; }
    public string? NomeArquivoFoto { get; set; }
}
