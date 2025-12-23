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
                    id AS ""Id"",
                    nome AS ""Nome"",
                    telefone AS ""Telefone"",
                    email AS ""Email""
                FROM cliente.clientes
                ORDER BY nome";

            return await connection.QueryAsync<ClienteDto>(sql);
        }

        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    id AS ""Id"",
                    nome AS ""Nome"",
                    telefone AS ""Telefone"",
                    email AS ""Email""
                FROM cliente.clientes
                WHERE id = @Id";

            return await connection.QueryFirstOrDefaultAsync<ClienteDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ClienteDto>> GetByNomeAsync(string nome)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    id AS ""Id"",
                    nome AS ""Nome"",
                    telefone AS ""Telefone"",
                    email AS ""Email""
                FROM cliente.clientes
                WHERE nome ILIKE @Nome
                ORDER BY nome";

            return await connection.QueryAsync<ClienteDto>(sql, new { Nome = $"%{nome}%" });
        }
    }
}
