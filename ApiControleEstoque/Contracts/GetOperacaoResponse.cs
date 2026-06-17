namespace ApiControleEstoque.Contracts
{
    public class GetOperacaoResponse
    {
        public Guid Id { get; set; }
        public DateTime Hora { get; set; }

        public required string Motivo { get; set;}
    }
}
