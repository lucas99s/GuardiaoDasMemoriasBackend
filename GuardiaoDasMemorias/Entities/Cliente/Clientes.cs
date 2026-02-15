using GuardiaoDasMemorias.Entities.Memoria;

namespace GuardiaoDasMemorias.Entities.Cliente
{
    public class Clientes
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Telefone { get; set; }
        public required string UserId { get; set; }
        public Models.ApplicationUser User { get; set; } = new Models.ApplicationUser();
        public ICollection<Memorias> Memorias { get; set; } = new List<Memorias>();
    }
}
