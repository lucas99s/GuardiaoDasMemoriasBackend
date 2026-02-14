namespace GuardiaoDasMemorias.Entities
{
    public class Template
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public bool Ativo { get; set; } = true;
        public int TemaId { get; set; }
        public Tema? Tema { get; set; }
        public ICollection<Memoria> Memorias { get; set; } = new List<Memoria>();
    }
}
