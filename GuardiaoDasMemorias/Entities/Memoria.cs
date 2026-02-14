namespace GuardiaoDasMemorias.Entities
{
    public class Memoria
    {
        public int Id { get; set; }
        public int TemaId { get; set; }
        public int TemplateId { get; set; }
        public int ClienteId { get; set; }
        public required string MemoriaHash { get; set; }
        public Cliente? Cliente { get; set; }
        public Tema? Tema { get; set; }
        public Template? Template { get; set; }
        public ICollection<Musica> Musicas { get; set; } = new List<Musica>();
    }
}
