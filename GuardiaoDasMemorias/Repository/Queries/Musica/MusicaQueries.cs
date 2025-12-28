using Dapper;
using GuardiaoDasMemorias.DTOs.Musica;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Queries.Musica
{
    public class MusicaQueries : IMusicaQueries
    {
        private readonly string _connectionString;

        public MusicaQueries(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<MusicaDto>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    id AS ""Id"",
                    nome AS ""Nome"",
                    caminho AS ""Caminho"",
                    memoria_id AS ""MemoriaId""
                FROM musica.musicas
                ORDER BY nome";

            return await connection.QueryAsync<MusicaDto>(sql);
        }

        public async Task<MusicaDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    id AS ""Id"",
                    nome AS ""Nome"",
                    caminho AS ""Caminho"",
                    memoria_id AS ""MemoriaId""
                FROM musica.musicas
                WHERE id = @Id";

            return await connection.QueryFirstOrDefaultAsync<MusicaDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<MusicaDto>> GetByMemoriaHashAsync(string memoriaHash)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    A.id AS ""Id"",
                    A.nome AS ""Nome"",
                    A.caminho AS ""Caminho"",
                    A.memoria_id AS ""MemoriaId""
                FROM musica.musicas A
                INNER JOIN memoria.memorias B ON A.memoria_id = B.id
                WHERE B.memoria_hash = @MemoriaHash
                ORDER BY A.nome";

            return await connection.QueryAsync<MusicaDto>(sql, new { MemoriaHash = memoriaHash });
        }

        public async Task<IEnumerable<MusicaDto>> GetByNomeAsync(string nome)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    id AS ""Id"",
                    nome AS ""Nome"",
                    caminho AS ""Caminho"",
                    memoria_id AS ""MemoriaId""
                FROM musica.musicas
                WHERE nome ILIKE @Nome
                ORDER BY nome";

            return await connection.QueryAsync<MusicaDto>(sql, new { Nome = $"%{nome}%" });
        }
    }
}
