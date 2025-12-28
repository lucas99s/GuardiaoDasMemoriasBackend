namespace GuardiaoDasMemorias.DTOs.Musica
{
    public class MusicaDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Caminho { get; set; }
        public int MemoriaId { get; set; }
    }
}
