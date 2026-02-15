using Dapper;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Commands.Template
{
    public class TemplateCommands : ITemplateCommands
    {
        private readonly string _connectionString;

        public TemplateCommands(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<int> CreateAsync(Entities.Template.Templates template)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                INSERT INTO template.templates (nome, ativo, tema_id)
                VALUES (@Nome, @Ativo, @TemaId)
                RETURNING id";

            return await connection.ExecuteScalarAsync<int>(sql, template);
        }

        public async Task<bool> UpdateAsync(Entities.Template.Templates template)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                UPDATE template.templates
                SET nome = @Nome,
                    ativo = @Ativo,
                    tema_id = @TemaId
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, template);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"DELETE FROM template.templates WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> ToggleAtivoAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                UPDATE template.templates
                SET ativo = NOT ativo
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
