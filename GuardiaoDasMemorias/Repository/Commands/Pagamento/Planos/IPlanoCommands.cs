namespace GuardiaoDasMemorias.Repository.Commands.Pagamento.Planos
{
    public interface IPlanoCommands
    {
        Task<int> CreateAsync(Entities.Pagamentos.Planos plano);
        Task<bool> UpdateAsync(Entities.Pagamentos.Planos plano);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<bool> ActivateAsync(int id);
        Task<bool> AddLimiteAsync(Entities.Pagamentos.PlanoLimites limite);
        Task<bool> RemoveLimiteAsync(int id);
        Task<bool> UpdateLimiteAsync(Entities.Pagamentos.PlanoLimites limite);
        Task<bool> AddRecursoAsync(Entities.Pagamentos.PlanoRecursos recurso);
        Task<bool> RemoveRecursoAsync(int id);
        Task<bool> UpdateRecursoAsync(Entities.Pagamentos.PlanoRecursos recurso);
    }
}
