using Dapper;
using Npgsql;

namespace GuardiaoDasMemorias.Services
{
    public class HashService : IHashService
    {
        private readonly string _connectionString;
        private const int HashLength = 10; // Tamanho do hash (ex: "a3f7c2d9e1")

        public HashService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> GenerateUniqueHashAsync()
        {
            string hash;
            bool isUnique;
            int maxAttempts = 10; // Reduzido porque a chance de colisão é mínima
            int attempts = 0;

            do
            {
                hash = GenerateHashFromGuid(HashLength);
                isUnique = await IsHashUniqueAsync(hash);
                attempts++;

                if (attempts >= maxAttempts)
                {
                    // Fallback: adiciona timestamp se necessário
                    hash = GenerateHashWithTimestamp(HashLength);
                    isUnique = await IsHashUniqueAsync(hash);
                    
                    if (!isUnique)
                    {
                        throw new InvalidOperationException("Não foi possível gerar um hash único após várias tentativas.");
                    }
                }
            }
            while (!isUnique);

            return hash;
        }

        /// <summary>
        /// Gera um hash baseado em GUID (Globally Unique Identifier)
        /// Usa apenas caracteres alfanuméricos lowercase para URL-friendly
        /// </summary>
        private string GenerateHashFromGuid(int length)
        {
            var guid = Guid.NewGuid();
            var base64 = Convert.ToBase64String(guid.ToByteArray())
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "")
                .ToLower();
            
            // Pega apenas os primeiros 'length' caracteres
            return base64.Substring(0, Math.Min(length, base64.Length));
        }

        /// <summary>
        /// Gera um hash combinando GUID com timestamp (fallback)
        /// Praticamente impossível gerar duplicatas
        /// </summary>
        private string GenerateHashWithTimestamp(int length)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var guid = Guid.NewGuid();
            var combined = $"{guid}{timestamp}";
            
            // Gera hash SHA256 do combinado
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combined));
            var base64 = Convert.ToBase64String(hashBytes)
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "")
                .ToLower();
            
            return base64.Substring(0, Math.Min(length, base64.Length));
        }

        private async Task<bool> IsHashUniqueAsync(string hash)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT COUNT(1)
                FROM memoria.memorias
                WHERE memoria_hash = @Hash";

            var count = await connection.ExecuteScalarAsync<int>(sql, new { Hash = hash });
            
            return count == 0;
        }
    }
}
