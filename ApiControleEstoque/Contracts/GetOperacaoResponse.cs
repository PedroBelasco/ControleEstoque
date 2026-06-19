using OperacaoDomain;
using ProdutoDomain;

namespace ApiControleEstoque.Contracts
{
    public class GetOperacaoResponse
    {
        public Guid Id { get; set; }
        public DateTime Hora { get; set; }

        public required string EntradaSaida { get; set; }
        public required string Motivo { get; set;}

        public required List<OperacaoEstoqueDetalheDTO> Detalhes { get; set; } = [];

    }
        public class OperacaoEstoqueDetalheDTO {

            public required Guid Id { get; set; }
            public required decimal Quantidade { get; set; }
            
            public required string NomeProduto { get; set; }
            public required string Sigla { get; set; }


        }
}
