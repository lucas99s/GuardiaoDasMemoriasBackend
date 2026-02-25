using Dapper;
using GuardiaoDasMemorias.Entities.Plano;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Commands.Planos
{
    public class PlanoCommands : IPlanoCommands
    {
        private readonly string _connectionString;

        public PlanoCommands(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Cria um novo plano
        /// </summary>
        public async Task<int> CreateAsync(Entities.Plano.Planos plano)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                INSERT INTO pagamento.planos 
                    (tema_id, tipo_pagamento_id, code, nome, descricao, preco, ativo, ordem, criado, atualizado)
                VALUES 
                    (@TemaId, @TipoPagamentoId, @Code, @Nome, @Descricao, @Preco, @Ativo, @Ordem, @Criado, @Atualizado)
                RETURNING id";

            return await connection.ExecuteScalarAsync<int>(sql, plano);
        }

        /// <summary>
        /// Atualiza um plano existente
        /// </summary>
        public async Task<bool> UpdateAsync(Entities.Plano.Planos plano)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.planos
                SET 
                    tema_id = @TemaId,
                    tipo_pagamento_id = @TipoPagamentoId,
                    code = @Code,
                    nome = @Nome,
                    descricao = @Descricao,
                    preco = @Preco,
                    ativo = @Ativo,
                    ordem = @Ordem,
                    atualizado = @Atualizado
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, plano);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Deleta um plano (remove completamente do banco)
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"DELETE FROM pagamento.planos WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        /// <summary>
        /// Desativa um plano (soft delete via ativo = false)
        /// </summary>
        public async Task<bool> DeactivateAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.planos
                SET ativo = false, atualizado = @DataAtualizado
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, 
                new { Id = id, DataAtualizado = DateTime.UtcNow });
            return rowsAffected > 0;
        }

        /// <summary>
        /// Ativa um plano
        /// </summary>
        public async Task<bool> ActivateAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.planos
                SET ativo = true, atualizado = @DataAtualizado
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, 
                new { Id = id, DataAtualizado = DateTime.UtcNow });
            return rowsAffected > 0;
        }

        /// <summary>
        /// Adiciona um limite a um plano
        /// </summary>
        public async Task<bool> AddLimiteAsync(PlanoLimites limite)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                INSERT INTO pagamento.plano_limites 
                    (plano_id, propriedade, valor, descricao)
                VALUES 
                    (@PlanoId, @Propriedade, @Valor, @Descricao)";

            var rowsAffected = await connection.ExecuteAsync(sql, limite);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Remove um limite de um plano
        /// </summary>
        public async Task<bool> RemoveLimiteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"DELETE FROM pagamento.plano_limites WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        /// <summary>
        /// Atualiza um limite de um plano
        /// </summary>
        public async Task<bool> UpdateLimiteAsync(PlanoLimites limite)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.plano_limites
                SET 
                    propriedade = @Propriedade,
                    valor = @Valor,
                    descricao = @Descricao
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, limite);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Adiciona um recurso a um plano
        /// </summary>
        public async Task<bool> AddRecursoAsync(PlanoRecursos recurso)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                INSERT INTO pagamento.plano_recursos 
                    (plano_id, recurso_key, descricao, ativo, ordem)
                VALUES 
                    (@PlanoId, @RecursoKey, @Descricao, @Ativo, @Ordem)";

            var rowsAffected = await connection.ExecuteAsync(sql, recurso);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Remove um recurso de um plano
        /// </summary>
        public async Task<bool> RemoveRecursoAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"DELETE FROM pagamento.plano_recursos WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        /// <summary>
        /// Atualiza um recurso de um plano
        /// </summary>
        public async Task<bool> UpdateRecursoAsync(PlanoRecursos recurso)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE pagamento.plano_recursos
                SET 
                    recurso_key = @RecursoKey,
                    descricao = @Descricao,
                    ativo = @Ativo,
                    ordem = @Ordem
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, recurso);
            return rowsAffected > 0;
        }
    }
}
