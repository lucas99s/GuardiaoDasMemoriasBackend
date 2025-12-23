using Dapper;
using GuardiaoDasMemorias.Entities;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Commands
{
    public class TemaCommands : ITemaCommands
    {
        private readonly string _connectionString;

        public TemaCommands(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<int> CreateAsync(Tema tema)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                INSERT INTO tema.temas (nome)
                VALUES (@Nome)
                RETURNING id";

            return await connection.ExecuteScalarAsync<int>(sql, tema);
        }

        public async Task<bool> UpdateAsync(Tema tema)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                UPDATE tema.temas
                SET nome = @Nome
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, tema);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"DELETE FROM tema.temas WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
