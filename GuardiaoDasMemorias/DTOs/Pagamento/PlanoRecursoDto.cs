namespace GuardiaoDasMemorias.DTOs.Pagamento
{
    public class PlanoRecursoDto
    {
        public int Id { get; set; }
        public int PlanoId { get; set; }
        public string RecursoKey { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
    }
}
