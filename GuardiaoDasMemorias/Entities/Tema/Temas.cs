using GuardiaoDasMemorias.Entities.Memoria;
using GuardiaoDasMemorias.Entities.Template;

namespace GuardiaoDasMemorias.Entities.Tema
{
    public class Temas
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public ICollection<Templates> Templates { get; set; } = new List<Templates>();
        public ICollection<Memorias> Memorias { get; set; } = new List<Memorias>();
    }
}
