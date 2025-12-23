using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Repository.Commands
{
    public interface ITemaCommands
    {
        Task<int> CreateAsync(Tema tema);
        Task<bool> UpdateAsync(Tema tema);
        Task<bool> DeleteAsync(int id);
    }
}
