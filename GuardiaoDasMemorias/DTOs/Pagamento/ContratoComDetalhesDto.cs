namespace GuardiaoDasMemorias.DTOs.Pagamento
{
    public class ContratoComDetalhesDto
    {
        public int Id { get; set; }
        public int MemoriaId { get; set; }
        public string MemoriaNome { get; set; } = string.Empty;
        public int PlanoId { get; set; }
        public string PlanoNome { get; set; } = string.Empty;
        public decimal PlanoPreco { get; set; }
        public int ContratoStatusId { get; set; }
        public string StatusNome { get; set; } = string.Empty;
        public int ContratoOrigemId { get; set; }
        public string OrigemNome { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public decimal ValorPago { get; set; }
        public string? TransacaoId { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? PagoEm { get; set; }
        public DateTime? ExpiraEm { get; set; }
        public DateTime? CanceladoEm { get; set; }
    }
}
