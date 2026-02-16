namespace GuardiaoDasMemorias.Entities.Pagamentos
{
    public class PlanoRecursos
    {
        public int Id { get; set; }
        public int PlanoId { get; set; }
        public required string RecursoKey { get; set; }
        public required string Descricao { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public required Planos Plano { get; set; }
    }

    // Exemplo de uso: Recurso = "criacao_qr_code", Descricao = "Possibilidade de criar um QR Code personalizado para a memória."
}
