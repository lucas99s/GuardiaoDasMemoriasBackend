namespace GuardiaoDasMemorias.Entities.Pagamentos
{
    public class ContratoStatus
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public ICollection<ContratoMemoria> ContratoMemorias { get; set; } = new List<ContratoMemoria>();
    }

    // status: "Ativo", "Cancelado", "Pendente", "Expirado", etc.
}
