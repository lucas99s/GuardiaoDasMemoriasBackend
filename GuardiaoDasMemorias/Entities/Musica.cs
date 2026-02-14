namespace GuardiaoDasMemorias.Entities
{
    public class Musica
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Caminho { get; set; }
        public int MemoriaId { get; set; }
        public Memoria? Memoria { get; set; }
    }
}
