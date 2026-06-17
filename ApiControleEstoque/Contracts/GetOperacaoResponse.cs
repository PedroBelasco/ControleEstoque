using OperacaoDomain;
using ProdutoDomain;

namespace ApiControleEstoque.Contracts
{
    public class GetOperacaoResponse
    {
        public Guid Id { get; set; }
        public DateTime Hora { get; set; }
        public required string Motivo { get; set;}

        public List<OperacaoEstoqueDetalheDTO> Detalhes { get; set; } = [];

    }
        public class OperacaoEstoqueDetalheDTO {

            public Guid Id { get; set; }
            public decimal Quantidade { get; set; }
            public string NomeProduto { get; set; }
            public string Sigla { get; set; }


        }
}
