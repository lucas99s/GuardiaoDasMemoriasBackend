using GuardiaoDasMemorias.Entities.Tema;
using GuardiaoDasMemorias.Entities.Memoria;

namespace GuardiaoDasMemorias.Entities.Template
{
    public class Templates
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public bool Ativo { get; set; } = true;
        public int TemaId { get; set; }
        public Temas? Tema { get; set; }
        public ICollection<Memorias> Memorias { get; set; } = new List<Memorias>();
    }
}
