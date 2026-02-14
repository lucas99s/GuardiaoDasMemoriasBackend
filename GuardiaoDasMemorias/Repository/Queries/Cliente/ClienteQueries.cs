using Dapper;
using GuardiaoDasMemorias.DTOs.Cliente;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Queries.Cliente
{
    public class ClienteQueries : IClienteQueries
    {
        private readonly string _connectionString;

        public ClienteQueries(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    c.id AS ""Id"",
                    c.nome AS ""Nome"",
                    c.telefone AS ""Telefone"",
                    c.user_id AS ""UserId"",
                    u.email AS ""Email""
                FROM cliente.clientes c
                INNER JOIN auth.users u ON c.user_id = u.id
                ORDER BY c.nome";

            return await connection.QueryAsync<ClienteDto>(sql);
        }

        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    c.id AS ""Id"",
                    c.nome AS ""Nome"",
                    c.telefone AS ""Telefone"",
                    c.user_id AS ""UserId"",
                    u.email AS ""Email""
                FROM cliente.clientes c
                INNER JOIN auth.users u ON c.user_id = u.id
                WHERE c.id = @Id";

            return await connection.QueryFirstOrDefaultAsync<ClienteDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ClienteDto>> GetByNomeAsync(string nome)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    c.id AS ""Id"",
                    c.nome AS ""Nome"",
                    c.telefone AS ""Telefone"",
                    c.user_id AS ""UserId"",
                    u.email AS ""Email""
                FROM cliente.clientes c
                INNER JOIN auth.users u ON c.user_id = u.id
                WHERE c.nome ILIKE @Nome
                ORDER BY c.nome";

            return await connection.QueryAsync<ClienteDto>(sql, new { Nome = $"%{nome}%" });
        }
    }
}
