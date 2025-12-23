using Dapper;
using GuardiaoDasMemorias.DTOs.Tema;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Queries.Tema
{
    public class TemaQueries : ITemaQueries
    {
        private readonly string _connectionString;

        public TemaQueries(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<TemaDto>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    id AS ""Id"",
                    nome AS ""Nome""
                FROM tema.temas
                ORDER BY nome";

            return await connection.QueryAsync<TemaDto>(sql);
        }

        public async Task<TemaDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    id AS ""Id"",
                    nome AS ""Nome""
                FROM tema.temas
                WHERE id = @Id";

            return await connection.QueryFirstOrDefaultAsync<TemaDto>(sql, new { Id = id });
        }
    }
}
