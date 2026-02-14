namespace GuardiaoDasMemorias.Entities
{
    public class Tema
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public ICollection<Template> Templates { get; set; } = new List<Template>();
        public ICollection<Memoria> Memorias { get; set; } = new List<Memoria>();
    }
}
