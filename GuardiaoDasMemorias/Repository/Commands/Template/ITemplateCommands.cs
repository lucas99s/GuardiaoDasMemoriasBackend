using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Repository.Commands.Template
{
    public interface ITemplateCommands
    {
        Task<int> CreateAsync(Entities.Template template);
        Task<bool> UpdateAsync(Entities.Template template);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleAtivoAsync(int id);
    }
}
