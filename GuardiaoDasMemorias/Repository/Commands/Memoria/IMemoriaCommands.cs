namespace GuardiaoDasMemorias.Repository.Commands.Memoria
{
    public interface IMemoriaCommands
    {
        Task<int> CreateAsync(Entities.Memoria.Memorias memoria);
        Task<bool> UpdateAsync(Entities.Memoria.Memorias memoria);
        Task<bool> DeleteAsync(int id);
    }
}
