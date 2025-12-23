using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Repository.Commands.Tema
{
    public interface ITemaCommands
    {
        Task<int> CreateAsync(Entities.Tema tema);
        Task<bool> UpdateAsync(Entities.Tema tema);
        Task<bool> DeleteAsync(int id);
    }
}
