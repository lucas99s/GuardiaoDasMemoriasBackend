using GuardiaoDasMemorias.Entities.Memoria;
using GuardiaoDasMemorias.Entities.Pagamentos;
using GuardiaoDasMemorias.Entities.Template;

namespace GuardiaoDasMemorias.Entities.Tema
{
    public class Temas
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public ICollection<Templates> Templates { get; set; } = new List<Templates>();
        public ICollection<Memorias> Memorias { get; set; } = new List<Memorias>();
        public ICollection<Planos> Planos { get; set; } = new List<Planos>();
    }
}
