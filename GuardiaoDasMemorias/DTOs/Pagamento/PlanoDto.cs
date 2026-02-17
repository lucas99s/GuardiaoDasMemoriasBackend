namespace GuardiaoDasMemorias.DTOs.Pagamento
{
    public class PlanoDto
    {
        public int Id { get; set; }
        public int TemaId { get; set; }
        public int TipoPagamentoId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        public DateTime Criado { get; set; }
        public DateTime? Atualizado { get; set; }
    }
}
