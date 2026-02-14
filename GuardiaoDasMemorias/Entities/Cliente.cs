namespace GuardiaoDasMemorias.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Telefone { get; set; }
        public required string UserId { get; set; }
        public Models.ApplicationUser User { get; set; } = new Models.ApplicationUser();
        public ICollection<Memoria> Memorias { get; set; } = new List<Memoria>();
    }
}
