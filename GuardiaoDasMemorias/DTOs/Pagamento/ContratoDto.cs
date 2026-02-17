namespace GuardiaoDasMemorias.DTOs.Pagamento
{
    public class ContratoDto
    {
        public int Id { get; set; }
        public int MemoriaId { get; set; }
        public int PlanoId { get; set; }
        public int ContratoStatusId { get; set; }
        public int ContratoOrigemId { get; set; }
        public int ClienteId { get; set; }
        public decimal ValorPago { get; set; }
        public string? TransacaoId { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? PagoEm { get; set; }
        public DateTime? ExpiraEm { get; set; }
        public DateTime? CanceladoEm { get; set; }
    }
}
