using GuardiaoDasMemorias.DTOs.Plano;

namespace GuardiaoDasMemorias.Repository.Queries.Planos
{
    public interface IPlanoQueries
    {
        // Planos
        Task<IEnumerable<PlanoDto>> GetAllAsync();
        Task<IEnumerable<PlanoDto>> GetActiveAsync();
        Task<PlanoDto?> GetByIdAsync(int id);
        Task<PlanoDto?> GetByCodeAsync(string code);
        Task<IEnumerable<PlanoDto>> GetByTemaAsync(int temaId);
        Task<IEnumerable<PlanoDto>> GetByTemaActiveAsync(int temaId);
        Task<IEnumerable<PlanoDto>> GetByTipoPagamentoAsync(int tipoPagamentoId);
        Task<PlanoComDetalhesDto?> GetComDetalhesAsync(int id);
        
        // Limites
        Task<IEnumerable<PlanoLimiteDto>> GetLimitesAsync(int planoId);
        Task<PlanoLimiteDto?> GetLimiteByIdAsync(int id);
        Task<PlanoLimiteDto?> GetLimiteByPropriedadeAsync(int planoId, string propriedade);
        
        // Recursos
        Task<IEnumerable<PlanoRecursoDto>> GetRecursosAsync(int planoId);
        Task<IEnumerable<PlanoRecursoDto>> GetRecursosAtivosAsync(int planoId);
        Task<PlanoRecursoDto?> GetRecursoByIdAsync(int id);
        Task<PlanoRecursoDto?> GetRecursoByKeyAsync(int planoId, string recursoKey);
    }
}
