namespace GuardiaoDasMemorias.Repository.Commands.Pagamento.Contratos
{
    public interface IContratoCommands
    {
        // ContratoMemoria
        Task<int> CreateAsync(Entities.Pagamentos.ContratoMemoria contrato);
        Task<bool> UpdateAsync(Entities.Pagamentos.ContratoMemoria contrato);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarkAsPaidAsync(int id, string? transacaoId);
        Task<bool> MarkAsCanceledAsync(int id);
        Task<bool> MarkAsExpiredAsync(int id);
        Task<bool> ChangeStatusAsync(int id, int novoStatusId);
        
        // ContratoHistorico
        Task<bool> CreateHistoricoAsync(Entities.Pagamentos.ContratoHistorico historico);
        Task<bool> ProcessUpgradeAsync(int contratoAntigoId, int contratoNovoId, string tipoMudanca, string? observacao);
    }
}
