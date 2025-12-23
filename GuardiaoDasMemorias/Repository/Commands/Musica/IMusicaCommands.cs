using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Repository.Commands.Musica
{
    public interface IMusicaCommands
    {
        Task<int> CreateAsync(Entities.Musica musica);
        Task<bool> UpdateAsync(Entities.Musica musica);
        Task<bool> DeleteAsync(int id);
    }
}
