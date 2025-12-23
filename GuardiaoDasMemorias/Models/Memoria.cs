namespace GuardiaoDasMemorias.Models;

public class Memoria
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataEvento { get; set; }
    public string? LocalEvento { get; set; }
    public List<string> Tags { get; set; } = new();
    public string? ImagemUrl { get; set; }
}
