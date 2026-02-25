namespace GuardiaoDasMemorias.DTOs.Plano
{
    public class PlanoLimiteDto
    {
        public int Id { get; set; }
        public int PlanoId { get; set; }
        public string Propriedade { get; set; } = string.Empty;
        public int Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
