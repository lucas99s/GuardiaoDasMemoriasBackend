using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Repository.Commands.Memoria
{
    public interface IMemoriaCommands
    {
        Task<int> CreateAsync(Entities.Memoria memoria);
        Task<bool> UpdateAsync(Entities.Memoria memoria);
        Task<bool> DeleteAsync(int id);
    }
}
