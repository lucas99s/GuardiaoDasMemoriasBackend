using GuardiaoDasMemorias.Entities.Tema;

namespace GuardiaoDasMemorias.Entities.Pagamentos
{
    public class Planos
    {
        public int Id { get; set; }
        public int TemaId { get; set; }
        public int TipoPagamentoId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public DateTime Criado { get; set; } = DateTime.UtcNow;
        public DateTime? Atualizado { get; set; }
        public required Temas Tema { get; set; }
        public required TipoPagamento TipoPagamento { get; set; }
        public ICollection<ContratoMemoria> Contratos { get; set; } = new List<ContratoMemoria>();
        public ICollection<PlanoLimites> PlanoLimites { get; set; } = new List<PlanoLimites>();
        public ICollection<PlanoRecursos> PlanoRecursos { get; set; } = new List<PlanoRecursos>();
    }
}
