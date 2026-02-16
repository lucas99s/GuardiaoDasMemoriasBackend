namespace GuardiaoDasMemorias.Entities.Pagamentos
{
    public class ContratoOrigem
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public ICollection<ContratoMemoria> ContratoMemorias { get; set; } = new List<ContratoMemoria>();
    }

    // origem: "Compra no site", "Afiliado", "Presente admin".
}
