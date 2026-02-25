using GuardiaoDasMemorias.Entities.Contrato;

namespace GuardiaoDasMemorias.Repository.Commands.Contratos
{
    public interface IContratoCommands
    {
        // ContratoMemoria
        Task<int> CreateAsync(ContratoMemoria contrato);
        Task<bool> UpdateAsync(ContratoMemoria contrato);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarkAsPaidAsync(int id, string? transacaoId);
        Task<bool> MarkAsCanceledAsync(int id);
        Task<bool> MarkAsExpiredAsync(int id);
        Task<bool> ChangeStatusAsync(int id, int novoStatusId);
        
        // ContratoHistorico
        Task<bool> CreateHistoricoAsync(ContratoHistorico historico);
        Task<bool> ProcessUpgradeAsync(int contratoAntigoId, int contratoNovoId, string tipoMudanca, string? observacao);
    }
}
