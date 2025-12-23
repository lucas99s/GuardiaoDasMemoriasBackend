namespace GuardiaoDasMemorias.DTOs.Musica
{
    public class MusicaDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Caminho { get; set; }
        public int ClienteId { get; set; }
        public string? ClienteNome { get; set; }
    }
}
