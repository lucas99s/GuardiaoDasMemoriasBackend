namespace GuardiaoDasMemorias.Entities.Plano
{
    public class PlanoLimites
    {
        public int Id { get; set; }
        public int PlanoId { get; set; }
        public required string Propriedade { get; set; }
        public required int Valor { get; set; }
        public required string Descricao { get; set; }
        public required Planos Plano { get; set; }
    }

    // Exemplo de uso: Propridade = "MaxFotos", Valor = "100", Descricao = "Número máximo de fotos permitidas para este plano"
}
