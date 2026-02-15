using GuardiaoDasMemorias.Entities.Cliente;
using GuardiaoDasMemorias.Entities.Tema;
using GuardiaoDasMemorias.Entities.Template;
using GuardiaoDasMemorias.Entities.Musica;

namespace GuardiaoDasMemorias.Entities.Memoria
{
    public class Memorias
    {
        public int Id { get; set; }
        public int TemaId { get; set; }
        public int TemplateId { get; set; }
        public int ClienteId { get; set; }
        public required string MemoriaHash { get; set; }
        public Clientes? Cliente { get; set; }
        public Temas? Tema { get; set; }
        public Templates? Template { get; set; }
        public ICollection<Musicas> Musicas { get; set; } = new List<Musicas>();
    }
}
