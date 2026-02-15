namespace GuardiaoDasMemorias.Repository.Commands.Template
{
    public interface ITemplateCommands
    {
        Task<int> CreateAsync(Entities.Template.Templates template);
        Task<bool> UpdateAsync(Entities.Template.Templates template);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleAtivoAsync(int id);
    }
}
