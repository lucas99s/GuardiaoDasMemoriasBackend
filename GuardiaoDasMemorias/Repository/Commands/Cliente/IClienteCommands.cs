using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Repository.Commands.Cliente
{
    public interface IClienteCommands
    {
        Task<int> CreateAsync(Entities.Cliente cliente);
        Task<bool> UpdateAsync(Entities.Cliente cliente);
        Task<bool> DeleteAsync(int id);
    }
}
