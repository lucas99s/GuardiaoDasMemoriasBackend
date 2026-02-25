namespace GuardiaoDasMemorias.Entities.Plano
{
    public class TipoPagamento
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public ICollection<Planos> Planos { get; set; } = new List<Planos>();
    }
}
