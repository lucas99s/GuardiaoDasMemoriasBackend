namespace GuardiaoDasMemorias.DTOs.Pagamento
{
    public class ContratoHistoricoDto
    {
        public int Id { get; set; }
        public int ContratoAntigoId { get; set; }
        public int ContratoNovoId { get; set; }
        public string TipoMudanca { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public DateTime RealizadoEm { get; set; }
    }
}
