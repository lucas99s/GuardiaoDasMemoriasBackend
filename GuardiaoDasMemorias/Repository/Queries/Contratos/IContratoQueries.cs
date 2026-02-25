using GuardiaoDasMemorias.DTOs.Pagamento;

namespace GuardiaoDasMemorias.Repository.Queries.Contratos
{
    public interface IContratoQueries
    {
        // ContratoMemoria - Básico
        Task<ContratoDto?> GetByIdAsync(int id);
        Task<IEnumerable<ContratoDto>> GetAllAsync();
        Task<IEnumerable<ContratoDto>> GetByClienteAsync(int clienteId);
        Task<IEnumerable<ContratoDto>> GetByMemoriaAsync(int memoriaId);
        Task<ContratoDto?> GetContratoAtivoByMemoriaAsync(int memoriaId);
        
        // ContratoMemoria - Por Status
        Task<IEnumerable<ContratoDto>> GetByStatusAsync(int statusId);
        Task<IEnumerable<ContratoDto>> GetPendentesAsync();
        Task<IEnumerable<ContratoDto>> GetAtivosAsync();
        Task<IEnumerable<ContratoDto>> GetCanceladosAsync();
        Task<IEnumerable<ContratoDto>> GetExpiradosAsync();
        
        // ContratoMemoria - Detalhes
        Task<ContratoComDetalhesDto?> GetComDetalhesAsync(int id);
        Task<IEnumerable<ContratoComDetalhesDto>> GetComDetalhesClienteAsync(int clienteId);
        
        // ContratoMemoria - Filtros Avançados
        Task<IEnumerable<ContratoDto>> GetProxiamosAExpirarAsync(int diasAntes = 7);
        Task<IEnumerable<ContratoDto>> GetByTransacaoAsync(string transacaoId);
        Task<ContratoDto?> GetByTransacaoAsync(string transacaoId, int clienteId);
        
        // ContratoHistorico
        Task<IEnumerable<ContratoHistoricoDto>> GetHistoricoAsync(int contratoId);
        Task<ContratoHistoricoDto?> GetHistoricoOrigemAsync(int contratoId);
        Task<ContratoHistoricoDto?> GetHistoricoDestinoAsync(int contratoId);
        Task<IEnumerable<ContratoHistoricoDto>> GetHistoricoByTipoAsync(string tipoMudanca);
    }
}
