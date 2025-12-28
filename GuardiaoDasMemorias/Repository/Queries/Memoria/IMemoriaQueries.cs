using GuardiaoDasMemorias.DTOs.Memoria;

namespace GuardiaoDasMemorias.Repository.Queries.Memoria
{
    public interface IMemoriaQueries
    {
        Task<IEnumerable<MemoriaDto>> GetAllAsync();
        Task<MemoriaDto?> GetByIdAsync(int id);
        Task<MemoriaDto?> GetByHashAsync(string hash);
        Task<IEnumerable<MemoriaDto>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<MemoriaDto>> GetByTemaIdAsync(int temaId);
    }
}
