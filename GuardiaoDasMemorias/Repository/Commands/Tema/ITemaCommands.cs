namespace GuardiaoDasMemorias.Repository.Commands.Tema
{
    public interface ITemaCommands
    {
        Task<int> CreateAsync(Entities.Tema.Temas tema);
        Task<bool> UpdateAsync(Entities.Tema.Temas tema);
        Task<bool> DeleteAsync(int id);
    }
}
