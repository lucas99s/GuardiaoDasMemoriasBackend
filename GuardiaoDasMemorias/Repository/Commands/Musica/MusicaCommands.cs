using Dapper;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Commands.Musica
{
    public class MusicaCommands : IMusicaCommands
    {
        private readonly string _connectionString;

        public MusicaCommands(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<int> CreateAsync(Entities.Musica.Musicas musica)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                INSERT INTO musica.musicas (nome, caminho, memoria_id)
                VALUES (@Nome, @Caminho, @MemoriaId)
                RETURNING id";

            return await connection.ExecuteScalarAsync<int>(sql, musica);
        }

        public async Task<bool> UpdateAsync(Entities.Musica.Musicas musica)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                UPDATE musica.musicas
                SET nome = @Nome,
                    caminho = @Caminho,
                    memoria_id = @MemoriaId
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, musica);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"DELETE FROM musica.musicas WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
