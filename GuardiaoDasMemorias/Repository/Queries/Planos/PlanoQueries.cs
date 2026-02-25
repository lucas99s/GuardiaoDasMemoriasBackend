using Dapper;
using GuardiaoDasMemorias.DTOs.Plano;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Queries.Planos
{
    public class PlanoQueries : IPlanoQueries
    {
        private readonly string _connectionString;

        public PlanoQueries(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        #region Planos

        /// <summary>
        /// Retorna todos os planos
        /// </summary>
        public async Task<IEnumerable<PlanoDto>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    tema_id AS ""TemaId"",
                    tipo_pagamento_id AS ""TipoPagamentoId"",
                    code AS ""Code"",
                    nome AS ""Nome"",
                    descricao AS ""Descricao"",
                    preco AS ""Preco"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem"",
                    criado AS ""Criado"",
                    atualizado AS ""Atualizado""
                FROM pagamento.planos
                ORDER BY ordem, nome";

            return await connection.QueryAsync<PlanoDto>(sql);
        }

        /// <summary>
        /// Retorna apenas planos ativos
        /// </summary>
        public async Task<IEnumerable<PlanoDto>> GetActiveAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    tema_id AS ""TemaId"",
                    tipo_pagamento_id AS ""TipoPagamentoId"",
                    code AS ""Code"",
                    nome AS ""Nome"",
                    descricao AS ""Descricao"",
                    preco AS ""Preco"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem"",
                    criado AS ""Criado"",
                    atualizado AS ""Atualizado""
                FROM pagamento.planos
                WHERE ativo = true
                ORDER BY ordem, nome";

            return await connection.QueryAsync<PlanoDto>(sql);
        }

        /// <summary>
        /// Retorna um plano por ID
        /// </summary>
        public async Task<PlanoDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    tema_id AS ""TemaId"",
                    tipo_pagamento_id AS ""TipoPagamentoId"",
                    code AS ""Code"",
                    nome AS ""Nome"",
                    descricao AS ""Descricao"",
                    preco AS ""Preco"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem"",
                    criado AS ""Criado"",
                    atualizado AS ""Atualizado""
                FROM pagamento.planos
                WHERE id = @Id";

            return await connection.QueryFirstOrDefaultAsync<PlanoDto>(sql, new { Id = id });
        }

        /// <summary>
        /// Retorna um plano por código (único)
        /// </summary>
        public async Task<PlanoDto?> GetByCodeAsync(string code)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    tema_id AS ""TemaId"",
                    tipo_pagamento_id AS ""TipoPagamentoId"",
                    code AS ""Code"",
                    nome AS ""Nome"",
                    descricao AS ""Descricao"",
                    preco AS ""Preco"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem"",
                    criado AS ""Criado"",
                    atualizado AS ""Atualizado""
                FROM pagamento.planos
                WHERE code = @Code";

            return await connection.QueryFirstOrDefaultAsync<PlanoDto>(sql, new { Code = code });
        }

        /// <summary>
        /// Retorna planos de um tema específico
        /// </summary>
        public async Task<IEnumerable<PlanoDto>> GetByTemaAsync(int temaId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    tema_id AS ""TemaId"",
                    tipo_pagamento_id AS ""TipoPagamentoId"",
                    code AS ""Code"",
                    nome AS ""Nome"",
                    descricao AS ""Descricao"",
                    preco AS ""Preco"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem"",
                    criado AS ""Criado"",
                    atualizado AS ""Atualizado""
                FROM pagamento.planos
                WHERE tema_id = @TemaId
                ORDER BY ordem, nome";

            return await connection.QueryAsync<PlanoDto>(sql, new { TemaId = temaId });
        }

        /// <summary>
        /// Retorna planos ativos de um tema específico
        /// </summary>
        public async Task<IEnumerable<PlanoDto>> GetByTemaActiveAsync(int temaId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    tema_id AS ""TemaId"",
                    tipo_pagamento_id AS ""TipoPagamentoId"",
                    code AS ""Code"",
                    nome AS ""Nome"",
                    descricao AS ""Descricao"",
                    preco AS ""Preco"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem"",
                    criado AS ""Criado"",
                    atualizado AS ""Atualizado""
                FROM pagamento.planos
                WHERE tema_id = @TemaId AND ativo = true
                ORDER BY ordem, nome";

            return await connection.QueryAsync<PlanoDto>(sql, new { TemaId = temaId });
        }

        /// <summary>
        /// Retorna planos por tipo de pagamento
        /// </summary>
        public async Task<IEnumerable<PlanoDto>> GetByTipoPagamentoAsync(int tipoPagamentoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    tema_id AS ""TemaId"",
                    tipo_pagamento_id AS ""TipoPagamentoId"",
                    code AS ""Code"",
                    nome AS ""Nome"",
                    descricao AS ""Descricao"",
                    preco AS ""Preco"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem"",
                    criado AS ""Criado"",
                    atualizado AS ""Atualizado""
                FROM pagamento.planos
                WHERE tipo_pagamento_id = @TipoPagamentoId
                ORDER BY ordem, nome";

            return await connection.QueryAsync<PlanoDto>(sql, new { TipoPagamentoId = tipoPagamentoId });
        }

        /// <summary>
        /// Retorna plano com seus limites e recursos
        /// </summary>
        public async Task<PlanoComDetalhesDto?> GetComDetalhesAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    p.id AS ""Id"",
                    p.tema_id AS ""TemaId"",
                    t.nome AS ""TemaNome"",
                    p.tipo_pagamento_id AS ""TipoPagamentoId"",
                    tp.nome AS ""TipoPagamentoNome"",
                    p.code AS ""Code"",
                    p.nome AS ""Nome"",
                    p.descricao AS ""Descricao"",
                    p.preco AS ""Preco"",
                    p.ativo AS ""Ativo"",
                    p.ordem AS ""Ordem"",
                    p.criado AS ""Criado"",
                    p.atualizado AS ""Atualizado""
                FROM pagamento.planos p
                LEFT JOIN tema.temas t ON p.tema_id = t.id
                LEFT JOIN pagamento.tipo_pagamento tp ON p.tipo_pagamento_id = tp.id
                WHERE p.id = @Id";

            var plano = await connection.QueryFirstOrDefaultAsync<PlanoComDetalhesDto>(sql, new { Id = id });

            if (plano == null)
                return null;

            // Buscar limites
            plano.Limites = (await GetLimitesAsync(id)).ToList();

            // Buscar recursos ativos
            plano.Recursos = (await GetRecursosAtivosAsync(id)).ToList();

            return plano;
        }

        #endregion

        #region Limites

        /// <summary>
        /// Retorna todos os limites de um plano
        /// </summary>
        public async Task<IEnumerable<PlanoLimiteDto>> GetLimitesAsync(int planoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    plano_id AS ""PlanoId"",
                    propriedade AS ""Propriedade"",
                    valor AS ""Valor"",
                    descricao AS ""Descricao""
                FROM pagamento.plano_limites
                WHERE plano_id = @PlanoId
                ORDER BY propriedade";

            return await connection.QueryAsync<PlanoLimiteDto>(sql, new { PlanoId = planoId });
        }

        /// <summary>
        /// Retorna um limite por ID
        /// </summary>
        public async Task<PlanoLimiteDto?> GetLimiteByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    plano_id AS ""PlanoId"",
                    propriedade AS ""Propriedade"",
                    valor AS ""Valor"",
                    descricao AS ""Descricao""
                FROM pagamento.plano_limites
                WHERE id = @Id";

            return await connection.QueryFirstOrDefaultAsync<PlanoLimiteDto>(sql, new { Id = id });
        }

        /// <summary>
        /// Retorna um limite por propriedade e plano
        /// </summary>
        public async Task<PlanoLimiteDto?> GetLimiteByPropriedadeAsync(int planoId, string propriedade)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    plano_id AS ""PlanoId"",
                    propriedade AS ""Propriedade"",
                    valor AS ""Valor"",
                    descricao AS ""Descricao""
                FROM pagamento.plano_limites
                WHERE plano_id = @PlanoId AND propriedade = @Propriedade";

            return await connection.QueryFirstOrDefaultAsync<PlanoLimiteDto>(sql, 
                new { PlanoId = planoId, Propriedade = propriedade });
        }

        #endregion

        #region Recursos

        /// <summary>
        /// Retorna todos os recursos de um plano
        /// </summary>
        public async Task<IEnumerable<PlanoRecursoDto>> GetRecursosAsync(int planoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    plano_id AS ""PlanoId"",
                    recurso_key AS ""RecursoKey"",
                    descricao AS ""Descricao"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem""
                FROM pagamento.plano_recursos
                WHERE plano_id = @PlanoId
                ORDER BY ordem, recurso_key";

            return await connection.QueryAsync<PlanoRecursoDto>(sql, new { PlanoId = planoId });
        }

        /// <summary>
        /// Retorna apenas recursos ativos de um plano
        /// </summary>
        public async Task<IEnumerable<PlanoRecursoDto>> GetRecursosAtivosAsync(int planoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    plano_id AS ""PlanoId"",
                    recurso_key AS ""RecursoKey"",
                    descricao AS ""Descricao"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem""
                FROM pagamento.plano_recursos
                WHERE plano_id = @PlanoId AND ativo = true
                ORDER BY ordem, recurso_key";

            return await connection.QueryAsync<PlanoRecursoDto>(sql, new { PlanoId = planoId });
        }

        /// <summary>
        /// Retorna um recurso por ID
        /// </summary>
        public async Task<PlanoRecursoDto?> GetRecursoByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    plano_id AS ""PlanoId"",
                    recurso_key AS ""RecursoKey"",
                    descricao AS ""Descricao"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem""
                FROM pagamento.plano_recursos
                WHERE id = @Id";

            return await connection.QueryFirstOrDefaultAsync<PlanoRecursoDto>(sql, new { Id = id });
        }

        /// <summary>
        /// Retorna um recurso por chave e plano
        /// </summary>
        public async Task<PlanoRecursoDto?> GetRecursoByKeyAsync(int planoId, string recursoKey)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    id AS ""Id"",
                    plano_id AS ""PlanoId"",
                    recurso_key AS ""RecursoKey"",
                    descricao AS ""Descricao"",
                    ativo AS ""Ativo"",
                    ordem AS ""Ordem""
                FROM pagamento.plano_recursos
                WHERE plano_id = @PlanoId AND recurso_key = @RecursoKey";

            return await connection.QueryFirstOrDefaultAsync<PlanoRecursoDto>(sql, 
                new { PlanoId = planoId, RecursoKey = recursoKey });
        }

        #endregion
    }
}
