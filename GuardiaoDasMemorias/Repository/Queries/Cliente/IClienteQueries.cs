using GuardiaoDasMemorias.DTOs.Cliente;

namespace GuardiaoDasMemorias.Repository.Queries.Cliente
{
    public interface IClienteQueries
    {
        Task<IEnumerable<ClienteDto>> GetAllAsync();
        Task<ClienteDto?> GetByIdAsync(int id);
        Task<IEnumerable<ClienteDto>> GetByNomeAsync(string nome);
    }
}
