using Dapper;
using GuardiaoDasMemorias.DTOs.Template;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Queries.Template
{
    public class TemplateQueries : ITemplateQueries
    {
        private readonly string _connectionString;

        public TemplateQueries(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<TemplateDto>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    t.id AS ""Id"",
                    t.nome AS ""Nome"",
                    t.ativo AS ""Ativo"",
                    t.tema_id AS ""TemaId"",
                    tm.nome AS ""TemaNome""
                FROM template.templates t
                LEFT JOIN tema.temas tm ON t.tema_id = tm.id
                ORDER BY t.nome";

            return await connection.QueryAsync<TemplateDto>(sql);
        }

        public async Task<TemplateDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    t.id AS ""Id"",
                    t.nome AS ""Nome"",
                    t.ativo AS ""Ativo"",
                    t.tema_id AS ""TemaId"",
                    tm.nome AS ""TemaNome""
                FROM template.templates t
                LEFT JOIN tema.temas tm ON t.tema_id = tm.id
                WHERE t.id = @Id";

            return await connection.QueryFirstOrDefaultAsync<TemplateDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<TemplateDto>> GetByTemaIdAsync(int temaId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    t.id AS ""Id"",
                    t.nome AS ""Nome"",
                    t.ativo AS ""Ativo"",
                    t.tema_id AS ""TemaId"",
                    tm.nome AS ""TemaNome""
                FROM template.templates t
                LEFT JOIN tema.temas tm ON t.tema_id = tm.id
                WHERE t.tema_id = @TemaId
                ORDER BY t.nome";

            return await connection.QueryAsync<TemplateDto>(sql, new { TemaId = temaId });
        }

        public async Task<IEnumerable<TemplateDto>> GetAtivosAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    t.id AS ""Id"",
                    t.nome AS ""Nome"",
                    t.ativo AS ""Ativo"",
                    t.tema_id AS ""TemaId"",
                    tm.nome AS ""TemaNome""
                FROM template.templates t
                LEFT JOIN tema.temas tm ON t.tema_id = tm.id
                WHERE t.ativo = true
                ORDER BY t.nome";

            return await connection.QueryAsync<TemplateDto>(sql);
        }
    }
}
