using Dapper;
using Npgsql;
using GuardiaoDasMemorias.Services;

namespace GuardiaoDasMemorias.Repository.Commands.Memoria
{
    public class MemoriaCommands : IMemoriaCommands
    {
        private readonly string _connectionString;
        private readonly IHashService _hashService;

        public MemoriaCommands(IConfiguration configuration, IHashService hashService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
        }

        public async Task<int> CreateAsync(Entities.Memoria memoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            // Gera um hash único para a memória
            var hash = await _hashService.GenerateUniqueHashAsync();
            memoria.MemoriaHash = hash;
            
            var sql = @"
                INSERT INTO memoria.memorias (tema_id, template_id, cliente_id, memoria_hash)
                VALUES (@TemaId, @TemplateId, @ClienteId, @MemoriaHash)
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
