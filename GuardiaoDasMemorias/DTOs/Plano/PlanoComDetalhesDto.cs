namespace GuardiaoDasMemorias.DTOs.Plano
{
    public class PlanoComDetalhesDto
    {
        public int Id { get; set; }
        public int TemaId { get; set; }
        public string TemaNome { get; set; } = string.Empty;
        public int TipoPagamentoId { get; set; }
        public string TipoPagamentoNome { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        public DateTime Criado { get; set; }
        public DateTime? Atualizado { get; set; }
        public List<PlanoLimiteDto> Limites { get; set; } = new();
        public List<PlanoRecursoDto> Recursos { get; set; } = new();
    }
}
