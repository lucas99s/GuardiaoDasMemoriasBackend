namespace GuardiaoDasMemorias.Repository.Commands.Musica
{
    public interface IMusicaCommands
    {
        Task<int> CreateAsync(Entities.Musica.Musicas musica);
        Task<bool> UpdateAsync(Entities.Musica.Musicas musica);
        Task<bool> DeleteAsync(int id);
    }
}
