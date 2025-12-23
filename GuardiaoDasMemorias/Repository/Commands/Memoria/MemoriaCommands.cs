using Dapper;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Commands.Memoria
{
    public class MemoriaCommands : IMemoriaCommands
    {
        private readonly string _connectionString;

        public MemoriaCommands(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<int> CreateAsync(Entities.Memoria memoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                INSERT INTO memoria.memorias (tema_id, template_id, cliente_id)
                VALUES (@TemaId, @TemplateId, @ClienteId)
                RETURNING id";

            return await connection.ExecuteScalarAsync<int>(sql, memoria);
        }

        public async Task<bool> UpdateAsync(Entities.Memoria memoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                UPDATE memoria.memorias
                SET tema_id = @TemaId,
                    template_id = @TemplateId,
                    cliente_id = @ClienteId
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, memoria);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"DELETE FROM memoria.memorias WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
