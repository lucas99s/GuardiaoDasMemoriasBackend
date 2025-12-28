using GuardiaoDasMemorias.DTOs.Musica;

namespace GuardiaoDasMemorias.Repository.Queries.Musica
{
    public interface IMusicaQueries
    {
        Task<IEnumerable<MusicaDto>> GetAllAsync();
        Task<MusicaDto?> GetByIdAsync(int id);
        Task<IEnumerable<MusicaDto>> GetByMemoriaHashAsync(string memoriaId);
        Task<IEnumerable<MusicaDto>> GetByNomeAsync(string nome);
    }
}
