using Dapper;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Commands.Cliente
{
    public class ClienteCommands : IClienteCommands
    {
        private readonly string _connectionString;

        public ClienteCommands(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<int> CreateAsync(Entities.Cliente cliente)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                INSERT INTO cliente.clientes (nome, telefone, email)
                VALUES (@Nome, @Telefone, @Email)
                RETURNING id";

            return await connection.ExecuteScalarAsync<int>(sql, cliente);
        }

        public async Task<bool> UpdateAsync(Entities.Cliente cliente)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                UPDATE cliente.clientes
                SET nome = @Nome,
                    telefone = @Telefone,
                    email = @Email
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, cliente);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"DELETE FROM cliente.clientes WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
