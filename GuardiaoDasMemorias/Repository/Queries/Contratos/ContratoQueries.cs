using Dapper;
using GuardiaoDasMemorias.DTOs.Pagamento;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Queries.Contratos
{
    public class ContratoQueries : IContratoQueries
    {
        private readonly string _connectionString;

        public ContratoQueries(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        #region ContratoMemoria - Básico

        /// <summary>
        /// Retorna um contrato por ID
        /// </summary>
        public async Task<ContratoDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                WHERE id = @Id";

            return await connection.QueryFirstOrDefaultAsync<ContratoDto>(sql, new { Id = id });
        }

        /// <summary>
        /// Retorna todos os contratos
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                ORDER BY criado_em DESC";

            return await connection.QueryAsync<ContratoDto>(sql);
        }

        /// <summary>
        /// Retorna contratos de um cliente
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetByClienteAsync(int clienteId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                WHERE cliente_id = @ClienteId
                ORDER BY criado_em DESC";

            return await connection.QueryAsync<ContratoDto>(sql, new { ClienteId = clienteId });
        }

        /// <summary>
        /// Retorna contratos de uma memória
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetByMemoriaAsync(int memoriaId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                WHERE memoria_id = @MemoriaId
                ORDER BY criado_em DESC";

            return await connection.QueryAsync<ContratoDto>(sql, new { MemoriaId = memoriaId });
        }

        /// <summary>
        /// Retorna o contrato ATIVO de uma memória (há apenas 1)
        /// Usa índice parcial único: ux_contrato_memoria_ativo_por_memoria
        /// </summary>
        public async Task<ContratoDto?> GetContratoAtivoByMemoriaAsync(int memoriaId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                WHERE memoria_id = @MemoriaId AND contrato_status_id = 2";

            return await connection.QueryFirstOrDefaultAsync<ContratoDto>(sql, new { MemoriaId = memoriaId });
        }

        #endregion

        #region ContratoMemoria - Por Status

        /// <summary>
        /// Retorna contratos com um status específico
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetByStatusAsync(int statusId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                WHERE contrato_status_id = @StatusId
                ORDER BY criado_em DESC";

            return await connection.QueryAsync<ContratoDto>(sql, new { StatusId = statusId });
        }

        /// <summary>
        /// Retorna contratos PENDENTES (status_id = 1)
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetPendentesAsync()
        {
            return await GetByStatusAsync(1);
        }

        /// <summary>
        /// Retorna contratos ATIVOS (status_id = 2)
        /// Usa índice parcial: ux_contrato_memoria_ativo_por_memoria
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetAtivosAsync()
        {
            return await GetByStatusAsync(2);
        }

        /// <summary>
        /// Retorna contratos CANCELADOS (status_id = 3)
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetCanceladosAsync()
        {
            return await GetByStatusAsync(3);
        }

        /// <summary>
        /// Retorna contratos EXPIRADOS (status_id = 4)
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetExpiradosAsync()
        {
            return await GetByStatusAsync(4);
        }

        #endregion

        #region ContratoMemoria - Detalhes

        /// <summary>
        /// Retorna contrato com detalhes de memória, plano, status e origem
        /// </summary>
        public async Task<ContratoComDetalhesDto?> GetComDetalhesAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    cm.id AS ""Id"",
                    cm.memoria_id AS ""MemoriaId"",
                    m.id AS ""MemoriaNome"",
                    cm.plano_id AS ""PlanoId"",
                    p.nome AS ""PlanoNome"",
                    p.preco AS ""PlanoPreco"",
                    cm.contrato_status_id AS ""ContratoStatusId"",
                    cs.nome AS ""StatusNome"",
                    cm.contrato_origem_id AS ""ContratoOrigemId"",
                    co.nome AS ""OrigemNome"",
                    cm.cliente_id AS ""ClienteId"",
                    c.nome AS ""ClienteNome"",
                    cm.valor_pago AS ""ValorPago"",
                    cm.transacao_id AS ""TransacaoId"",
                    cm.criado_em AS ""CriadoEm"",
                    cm.pago_em AS ""PagoEm"",
                    cm.expira_em AS ""ExpiraEm"",
                    cm.cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria cm
                LEFT JOIN memoria.memorias m ON cm.memoria_id = m.id
                LEFT JOIN pagamento.planos p ON cm.plano_id = p.id
                LEFT JOIN pagamento.contrato_status cs ON cm.contrato_status_id = cs.id
                LEFT JOIN pagamento.contrato_origem co ON cm.contrato_origem_id = co.id
                LEFT JOIN cliente.clientes c ON cm.cliente_id = c.id
                WHERE cm.id = @Id";

            var contrato = await connection.QueryFirstOrDefaultAsync<ContratoComDetalhesDto>(sql, new { Id = id });
            
            if (contrato != null)
            {
                // Buscar nome real da memória via query separada
                contrato.MemoriaNome = await GetMemoriaNomeAsync(contrato.MemoriaId);
            }
            
            return contrato;
        }

        /// <summary>
        /// Retorna contratos de um cliente com detalhes completos
        /// </summary>
        public async Task<IEnumerable<ContratoComDetalhesDto>> GetComDetalhesClienteAsync(int clienteId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    cm.id AS ""Id"",
                    cm.memoria_id AS ""MemoriaId"",
                    CAST(m.id AS VARCHAR) AS ""MemoriaNome"",
                    cm.plano_id AS ""PlanoId"",
                    p.nome AS ""PlanoNome"",
                    p.preco AS ""PlanoPreco"",
                    cm.contrato_status_id AS ""ContratoStatusId"",
                    cs.nome AS ""StatusNome"",
                    cm.contrato_origem_id AS ""ContratoOrigemId"",
                    co.nome AS ""OrigemNome"",
                    cm.cliente_id AS ""ClienteId"",
                    c.nome AS ""ClienteNome"",
                    cm.valor_pago AS ""ValorPago"",
                    cm.transacao_id AS ""TransacaoId"",
                    cm.criado_em AS ""CriadoEm"",
                    cm.pago_em AS ""PagoEm"",
                    cm.expira_em AS ""ExpiraEm"",
                    cm.cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria cm
                LEFT JOIN memoria.memorias m ON cm.memoria_id = m.id
                LEFT JOIN pagamento.planos p ON cm.plano_id = p.id
                LEFT JOIN pagamento.contrato_status cs ON cm.contrato_status_id = cs.id
                LEFT JOIN pagamento.contrato_origem co ON cm.contrato_origem_id = co.id
                LEFT JOIN cliente.clientes c ON cm.cliente_id = c.id
                WHERE cm.cliente_id = @ClienteId
                ORDER BY cm.criado_em DESC";

            return await connection.QueryAsync<ContratoComDetalhesDto>(sql, new { ClienteId = clienteId });
        }

        #endregion

        #region ContratoMemoria - Filtros Avançados

        /// <summary>
        /// Retorna contratos próximos de expirar (com assinatura)
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetProxiamosAExpirarAsync(int diasAntes = 7)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                WHERE 
                    contrato_status_id = 2 AND
                    expira_em IS NOT NULL AND
                    expira_em <= NOW() + INTERVAL '1 day' * @DiasAntes AND
                    expira_em > NOW()
                ORDER BY expira_em ASC";

            return await connection.QueryAsync<ContratoDto>(sql, new { DiasAntes = diasAntes });
        }

        /// <summary>
        /// Retorna contrato por TransacaoId (busca global)
        /// Usa índice: ux_contrato_memoria_transacao_id_not_null
        /// </summary>
        public async Task<IEnumerable<ContratoDto>> GetByTransacaoAsync(string transacaoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                WHERE transacao_id = @TransacaoId";

            return await connection.QueryAsync<ContratoDto>(sql, new { TransacaoId = transacaoId });
        }

        /// <summary>
        /// Retorna contrato por TransacaoId e ClienteId (busca segura)
        /// </summary>
        public async Task<ContratoDto?> GetByTransacaoAsync(string transacaoId, int clienteId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    memoria_id AS ""MemoriaId"",
                    plano_id AS ""PlanoId"",
                    contrato_status_id AS ""ContratoStatusId"",
                    contrato_origem_id AS ""ContratoOrigemId"",
                    cliente_id AS ""ClienteId"",
                    valor_pago AS ""ValorPago"",
                    transacao_id AS ""TransacaoId"",
                    criado_em AS ""CriadoEm"",
                    pago_em AS ""PagoEm"",
                    expira_em AS ""ExpiraEm"",
                    cancelado_em AS ""CanceladoEm""
                FROM pagamento.contrato_memoria
                WHERE transacao_id = @TransacaoId AND cliente_id = @ClienteId";

            return await connection.QueryFirstOrDefaultAsync<ContratoDto>(sql, 
                new { TransacaoId = transacaoId, ClienteId = clienteId });
        }

        #endregion

        #region ContratoHistorico

        /// <summary>
        /// Retorna histórico de mudanças de um contrato (como origem)
        /// </summary>
        public async Task<IEnumerable<ContratoHistoricoDto>> GetHistoricoAsync(int contratoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    contrato_antigo_id AS ""ContratoAntigoId"",
                    contrato_novo_id AS ""ContratoNovoId"",
                    tipo_mudanca AS ""TipoMudanca"",
                    observacao AS ""Observacao"",
                    realizado_em AS ""RealizadoEm""
                FROM pagamento.contrato_historico
                WHERE contrato_antigo_id = @ContratoId OR contrato_novo_id = @ContratoId
                ORDER BY realizado_em DESC";

            return await connection.QueryAsync<ContratoHistoricoDto>(sql, new { ContratoId = contratoId });
        }

        /// <summary>
        /// Retorna o histórico onde um contrato é a ORIGEM (foi substituído)
        /// </summary>
        public async Task<ContratoHistoricoDto?> GetHistoricoOrigemAsync(int contratoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    contrato_antigo_id AS ""ContratoAntigoId"",
                    contrato_novo_id AS ""ContratoNovoId"",
                    tipo_mudanca AS ""TipoMudanca"",
                    observacao AS ""Observacao"",
                    realizado_em AS ""RealizadoEm""
                FROM pagamento.contrato_historico
                WHERE contrato_antigo_id = @ContratoId
                ORDER BY realizado_em DESC
                LIMIT 1";

            return await connection.QueryFirstOrDefaultAsync<ContratoHistoricoDto>(sql, new { ContratoId = contratoId });
        }

        /// <summary>
        /// Retorna o histórico onde um contrato é o DESTINO (substituiu outro)
        /// </summary>
        public async Task<ContratoHistoricoDto?> GetHistoricoDestinoAsync(int contratoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    contrato_antigo_id AS ""ContratoAntigoId"",
                    contrato_novo_id AS ""ContratoNovoId"",
                    tipo_mudanca AS ""TipoMudanca"",
                    observacao AS ""Observacao"",
                    realizado_em AS ""RealizadoEm""
                FROM pagamento.contrato_historico
                WHERE contrato_novo_id = @ContratoId
                ORDER BY realizado_em DESC
                LIMIT 1";

            return await connection.QueryFirstOrDefaultAsync<ContratoHistoricoDto>(sql, new { ContratoId = contratoId });
        }

        /// <summary>
        /// Retorna históricos por tipo de mudança
        /// </summary>
        public async Task<IEnumerable<ContratoHistoricoDto>> GetHistoricoByTipoAsync(string tipoMudanca)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    contrato_antigo_id AS ""ContratoAntigoId"",
                    contrato_novo_id AS ""ContratoNovoId"",
                    tipo_mudanca AS ""TipoMudanca"",
                    observacao AS ""Observacao"",
                    realizado_em AS ""RealizadoEm""
                FROM pagamento.contrato_historico
                WHERE tipo_mudanca = @TipoMudanca
                ORDER BY realizado_em DESC";

            return await connection.QueryAsync<ContratoHistoricoDto>(sql, new { TipoMudanca = tipoMudanca });
        }

        #endregion

        #region Helpers Privados

        /// <summary>
        /// Busca o nome real da memória
        /// </summary>
        private async Task<string> GetMemoriaNomeAsync(int memoriaId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT CAST(id AS VARCHAR)
                FROM memoria.memorias
                WHERE id = @Id";

            var nome = await connection.QueryFirstOrDefaultAsync<string>(sql, new { Id = memoriaId });
            return nome ?? "Memória desconhecida";
        }

        #endregion
    }
}
