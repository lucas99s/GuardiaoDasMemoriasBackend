using GuardiaoDasMemorias.DTOs.Tema;

namespace GuardiaoDasMemorias.Repository.Queries.Tema
{
    public interface ITemaQueries
    {
        Task<IEnumerable<TemaDto>> GetAllAsync();
        Task<TemaDto?> GetByIdAsync(int id);
    }
}
