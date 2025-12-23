namespace GuardiaoDasMemorias.DTOs.Template
{
    public class TemplateDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public bool Ativo { get; set; } = true;
        public int TemaId { get; set; }
        public string? TemaNome { get; set; }
    }
}
