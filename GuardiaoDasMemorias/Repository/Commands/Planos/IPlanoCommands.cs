using GuardiaoDasMemorias.Entities.Plano;

namespace GuardiaoDasMemorias.Repository.Commands.Planos
{
    public interface IPlanoCommands
    {
        Task<int> CreateAsync(Entities.Plano.Planos plano);
        Task<bool> UpdateAsync(Entities.Plano.Planos plano);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<bool> ActivateAsync(int id);
        Task<bool> AddLimiteAsync(PlanoLimites limite);
        Task<bool> RemoveLimiteAsync(int id);
        Task<bool> UpdateLimiteAsync(PlanoLimites limite);
        Task<bool> AddRecursoAsync(PlanoRecursos recurso);
        Task<bool> RemoveRecursoAsync(int id);
        Task<bool> UpdateRecursoAsync(PlanoRecursos recurso);
    }
}
