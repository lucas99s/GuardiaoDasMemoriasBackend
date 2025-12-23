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
                    m.id AS ""Id"",
                    m.nome AS ""Nome"",
                    m.caminho AS ""Caminho"",
                    m.cliente_id AS ""ClienteId"",
                    c.nome AS ""ClienteNome""
                FROM musica.musicas m
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id
                ORDER BY m.nome";

            return await connection.QueryAsync<MusicaDto>(sql);
        }

        public async Task<MusicaDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    m.id AS ""Id"",
                    m.nome AS ""Nome"",
                    m.caminho AS ""Caminho"",
                    m.cliente_id AS ""ClienteId"",
                    c.nome AS ""ClienteNome""
                FROM musica.musicas m
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id
                WHERE m.id = @Id";

            return await connection.QueryFirstOrDefaultAsync<MusicaDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<MusicaDto>> GetByClienteIdAsync(int clienteId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    m.id AS ""Id"",
                    m.nome AS ""Nome"",
                    m.caminho AS ""Caminho"",
                    m.cliente_id AS ""ClienteId"",
                    c.nome AS ""ClienteNome""
                FROM musica.musicas m
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id
                WHERE m.cliente_id = @ClienteId
                ORDER BY m.nome";

            return await connection.QueryAsync<MusicaDto>(sql, new { ClienteId = clienteId });
        }

        public async Task<IEnumerable<MusicaDto>> GetByNomeAsync(string nome)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    m.id AS ""Id"",
                    m.nome AS ""Nome"",
                    m.caminho AS ""Caminho"",
                    m.cliente_id AS ""ClienteId"",
                    c.nome AS ""ClienteNome""
                FROM musica.musicas m
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id
                WHERE m.nome ILIKE @Nome
                ORDER BY m.nome";

            return await connection.QueryAsync<MusicaDto>(sql, new { Nome = $"%{nome}%" });
        }
    }
}
