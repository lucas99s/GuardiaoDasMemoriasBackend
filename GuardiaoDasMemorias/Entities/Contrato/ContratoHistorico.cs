namespace GuardiaoDasMemorias.Entities.Contrato
{
    public class ContratoHistorico
    {
        public int Id { get; set; }
        public int ContratoAntigoId { get; set; } // Contrato que foi substituído
        public int ContratoNovoId { get; set; } // Novo contrato criado (upgrade/downgrade)
        public required string TipoMudanca { get; set; } // "Upgrade", "Downgrade", "Renovação"
        public string? Observacao { get; set; } // Informações adicionais sobre a mudança
        public DateTime RealizadoEm { get; set; } = DateTime.UtcNow;

        public required ContratoMemoria ContratoAntigo { get; set; }
        public required ContratoMemoria ContratoNovo { get; set; }
    }
}
