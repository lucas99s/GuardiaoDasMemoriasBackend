using Dapper;
using GuardiaoDasMemorias.DTOs.Memoria;
using Npgsql;

namespace GuardiaoDasMemorias.Repository.Queries.Memoria
{
    public class MemoriaQueries : IMemoriaQueries
    {
        private readonly string _connectionString;

        public MemoriaQueries(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<MemoriaDto>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    m.id AS ""Id"",
                    m.tema_id AS ""TemaId"",
                    m.template_id AS ""TemplateId"",
                    m.cliente_id AS ""ClienteId"",
                    m.memoria_hash AS ""MemoriaHash"",
                    t.nome AS ""TemaNome"",
                    tp.nome AS ""TemplateName"",
                    c.nome AS ""ClienteNome""
                FROM memoria.memorias m
                LEFT JOIN tema.temas t ON m.tema_id = t.id
                LEFT JOIN template.templates tp ON m.template_id = tp.id
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id";

            return await connection.QueryAsync<MemoriaDto>(sql);
        }

        public async Task<MemoriaDto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    m.id AS ""Id"",
                    m.tema_id AS ""TemaId"",
                    m.template_id AS ""TemplateId"",
                    m.cliente_id AS ""ClienteId"",
                    m.memoria_hash AS ""MemoriaHash"",
                    t.nome AS ""TemaNome"",
                    tp.nome AS ""TemplateName"",
                    c.nome AS ""ClienteNome""
                FROM memoria.memorias m
                LEFT JOIN tema.temas t ON m.tema_id = t.id
                LEFT JOIN template.templates tp ON m.template_id = tp.id
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id
                WHERE m.id = @Id";

            return await connection.QueryFirstOrDefaultAsync<MemoriaDto>(sql, new { Id = id });
        }

        public async Task<MemoriaDto?> GetByHashAsync(string hash)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    m.id AS ""Id"",
                    m.tema_id AS ""TemaId"",
                    m.template_id AS ""TemplateId"",
                    m.cliente_id AS ""ClienteId"",
                    m.memoria_hash AS ""MemoriaHash"",
                    t.nome AS ""TemaNome"",
                    tp.nome AS ""TemplateName"",
                    c.nome AS ""ClienteNome""
                FROM memoria.memorias m
                LEFT JOIN tema.temas t ON m.tema_id = t.id
                LEFT JOIN template.templates tp ON m.template_id = tp.id
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id
                WHERE m.memoria_hash = @Hash";

            return await connection.QueryFirstOrDefaultAsync<MemoriaDto>(sql, new { Hash = hash });
        }

        public async Task<IEnumerable<MemoriaDto>> GetByClienteIdAsync(int clienteId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    m.id AS ""Id"",
                    m.tema_id AS ""TemaId"",
                    m.template_id AS ""TemplateId"",
                    m.cliente_id AS ""ClienteId"",
                    m.memoria_hash AS ""MemoriaHash"",
                    t.nome AS ""TemaNome"",
                    tp.nome AS ""TemplateName"",
                    c.nome AS ""ClienteNome""
                FROM memoria.memorias m
                LEFT JOIN tema.temas t ON m.tema_id = t.id
                LEFT JOIN template.templates tp ON m.template_id = tp.id
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id
                WHERE m.cliente_id = @ClienteId";

            return await connection.QueryAsync<MemoriaDto>(sql, new { ClienteId = clienteId });
        }

        public async Task<IEnumerable<MemoriaDto>> GetByTemaIdAsync(int temaId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    m.id AS ""Id"",
                    m.tema_id AS ""TemaId"",
                    m.template_id AS ""TemplateId"",
                    m.cliente_id AS ""ClienteId"",
                    m.memoria_hash AS ""MemoriaHash"",
                    t.nome AS ""TemaNome"",
                    tp.nome AS ""TemplateName"",
                    c.nome AS ""ClienteNome""
                FROM memoria.memorias m
                LEFT JOIN tema.temas t ON m.tema_id = t.id
                LEFT JOIN template.templates tp ON m.template_id = tp.id
                LEFT JOIN cliente.clientes c ON m.cliente_id = c.id
                WHERE m.tema_id = @TemaId";

            return await connection.QueryAsync<MemoriaDto>(sql, new { TemaId = temaId });
        }
    }
}
