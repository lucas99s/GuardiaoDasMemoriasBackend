using Dapper;
using GuardiaoDasMemorias.Entities.Contrato;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Commands.Contratos
{
    public class ContratoCommands : IContratoCommands
    {
        private readonly string _connectionString;

        public ContratoCommands(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Cria um novo contrato de memória
        /// </summary>
        public async Task<int> CreateAsync(ContratoMemoria contrato)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                INSERT INTO pagamento.contrato_memoria 
                    (memoria_id, plano_id, contrato_status_id, contrato_origem_id, cliente_id, 
                     valor_pago, transacao_id, criado_em, pago_em, expira_em, cancelado_em)
                VALUES 
                    (@MemoriaId, @PlanoId, @ContratoStatusId, @ContratoOrigemId, @ClienteId,
                     @ValorPago, @TransacaoId, @CriadoEm, @PagoEm, @ExpiraEm, @CanceladoEm)
                RETURNING id";

            return await connection.ExecuteScalarAsync<int>(sql, contrato);
        }

        /// <summary>
        /// Atualiza um contrato existente
        /// </summary>
        public async Task<bool> UpdateAsync(ContratoMemoria contrato)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.contrato_memoria
                SET 
                    memoria_id = @MemoriaId,
                    plano_id = @PlanoId,
                    contrato_status_id = @ContratoStatusId,
                    contrato_origem_id = @ContratoOrigemId,
                    cliente_id = @ClienteId,
                    valor_pago = @ValorPago,
                    transacao_id = @TransacaoId,
                    pago_em = @PagoEm,
                    expira_em = @ExpiraEm,
                    cancelado_em = @CanceladoEm
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, contrato);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Deleta um contrato (remove completamente do banco)
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"DELETE FROM pagamento.contrato_memoria WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        /// <summary>
        /// Marca um contrato como pago
        /// </summary>
        public async Task<bool> MarkAsPaidAsync(int id, string? transacaoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.contrato_memoria
                SET 
                    contrato_status_id = 2,
                    pago_em = @DataPagamento,
                    transacao_id = @TransacaoId
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, 
                new { Id = id, DataPagamento = DateTime.UtcNow, TransacaoId = transacaoId });
            
            return rowsAffected > 0;
        }

        /// <summary>
        /// Marca um contrato como cancelado
        /// </summary>
        public async Task<bool> MarkAsCanceledAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.contrato_memoria
                SET 
                    contrato_status_id = 3,
                    cancelado_em = @DataCancelamento
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, 
                new { Id = id, DataCancelamento = DateTime.UtcNow });
            
            return rowsAffected > 0;
        }

        /// <summary>
        /// Marca um contrato como expirado
        /// </summary>
        public async Task<bool> MarkAsExpiredAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.contrato_memoria
                SET contrato_status_id = 4
                WHERE id = @Id AND expira_em <= @DataAtual";

            var rowsAffected = await connection.ExecuteAsync(sql, 
                new { Id = id, DataAtual = DateTime.UtcNow });
            
            return rowsAffected > 0;
        }

        /// <summary>
        /// Altera o status de um contrato
        /// </summary>
        public async Task<bool> ChangeStatusAsync(int id, int novoStatusId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.contrato_memoria
                SET contrato_status_id = @NovoStatusId
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, 
                new { Id = id, NovoStatusId = novoStatusId });
            
            return rowsAffected > 0;
        }

        /// <summary>
        /// Cria um registro de histórico de mudança de contrato
        /// </summary>
        public async Task<bool> CreateHistoricoAsync(ContratoHistorico historico)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                INSERT INTO pagamento.contrato_historico 
                    (contrato_antigo_id, contrato_novo_id, tipo_mudanca, observacao, realizado_em)
                VALUES 
                    (@ContratoAntigoId, @ContratoNovoId, @TipoMudanca, @Observacao, @RealizadoEm)";

            var rowsAffected = await connection.ExecuteAsync(sql, historico);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Processa um upgrade/downgrade de plano em transação:
        /// 1. Cancela o contrato antigo
        /// 2. Cria novo contrato
        /// 3. Registra no histórico
        /// </summary>
        public async Task<bool> ProcessUpgradeAsync(int contratoAntigoId, int contratoNovoId, 
            string tipoMudanca, string? observacao)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            try
            {
                await connection.OpenAsync();

                using var transaction = connection.BeginTransaction();

                try
                {
                    // 1. Marca contrato antigo como cancelado
                    var sqlCancelar = @"
                        UPDATE pagamento.contrato_memoria
                        SET 
                            contrato_status_id = 3,
                            cancelado_em = @DataCancelamento
                        WHERE id = @ContratoAntigoId";

                    await connection.ExecuteAsync(sqlCancelar,
                        new { ContratoAntigoId = contratoAntigoId, DataCancelamento = DateTime.UtcNow },
                        transaction);

                    // 2. Marca novo contrato como ativo
                    var sqlAtivar = @"
                        UPDATE pagamento.contrato_memoria
                        SET contrato_status_id = 2
                        WHERE id = @ContratoNovoId";

                    await connection.ExecuteAsync(sqlAtivar,
                        new { ContratoNovoId = contratoNovoId },
                        transaction);

                    // 3. Cria registro de histórico
                    var sqlHistorico = @"
                        INSERT INTO pagamento.contrato_historico 
                            (contrato_antigo_id, contrato_novo_id, tipo_mudanca, observacao, realizado_em)
                        VALUES 
                            (@ContratoAntigoId, @ContratoNovoId, @TipoMudanca, @Observacao, @RealizadoEm)";

                    await connection.ExecuteAsync(sqlHistorico,
                        new
                        {
                            ContratoAntigoId = contratoAntigoId,
                            ContratoNovoId = contratoNovoId,
                            TipoMudanca = tipoMudanca,
                            Observacao = observacao,
                            RealizadoEm = DateTime.UtcNow
                        },
                        transaction);

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
