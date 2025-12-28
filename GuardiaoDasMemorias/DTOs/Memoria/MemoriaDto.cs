namespace GuardiaoDasMemorias.DTOs.Memoria
{
    public class MemoriaDto
    {
        public int Id { get; set; }
        public int TemaId { get; set; }
        public int TemplateId { get; set; }
        public int ClienteId { get; set; }
        public string? TemaNome { get; set; }
        public string? TemplateName { get; set; }
        public string? ClienteNome { get; set; }
        public string? MemoriaHash { get; set; }
    }
}
