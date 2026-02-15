using GuardiaoDasMemorias.Entities.Memoria;

namespace GuardiaoDasMemorias.Entities.Musica
{
    public class Musicas
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Caminho { get; set; }
        public int MemoriaId { get; set; }
        public Memorias? Memoria { get; set; }
    }
}
