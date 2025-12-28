namespace GuardiaoDasMemorias.Entities
{
    public class Musica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public int MemoriaId { get; set; }
    }
}
