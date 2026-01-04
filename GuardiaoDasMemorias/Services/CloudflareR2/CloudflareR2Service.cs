using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Options;

namespace GuardiaoDasMemorias.Services.CloudflareR2
{
    public class CloudflareR2Service : ICloudflareR2Service
    {
        private IAmazonS3? _s3Client;
        private readonly CloudflareR2Config _config;
        private readonly ILogger<CloudflareR2Service> _logger;
        private bool _isConfigured;
        private bool _clientInitialized = false;
        private readonly object _lock = new object();

        public CloudflareR2Service(
            IOptions<CloudflareR2Config> config,
            ILogger<CloudflareR2Service> logger)
        {
            _config = config.Value;
            _logger = logger;

            _isConfigured = !string.IsNullOrEmpty(_config.AccountId) &&
                           !string.IsNullOrEmpty(_config.AccessKeyId) &&
                           !string.IsNullOrEmpty(_config.SecretAccessKey) &&
                           !string.IsNullOrEmpty(_config.BucketName);

            if (!_isConfigured)
            {
                _logger.LogWarning("⚠️ Cloudflare R2 não configurado");
            }
        }

        private void EnsureClientInitialized()
        {
            if (_clientInitialized && _s3Client != null)
                return;

            lock (_lock)
            {
                if (_clientInitialized && _s3Client != null)
                    return;

                if (!_isConfigured)
                {
                    throw new InvalidOperationException(
                        "Cloudflare R2 não está configurado. " +
                        "Configure AccountId, AccessKeyId, SecretAccessKey e BucketName no appsettings.json");
                }

                try
                {
                    var credentials = new BasicAWSCredentials(_config.AccessKeyId, _config.SecretAccessKey);
                    var s3Config = new AmazonS3Config
                    {
                        ServiceURL = $"https://{_config.AccountId}.r2.cloudflarestorage.com",
                        ForcePathStyle = true
                    };

                    _s3Client = new AmazonS3Client(credentials, s3Config);
                    _clientInitialized = true;
                    _logger.LogInformation("✅ Cloudflare R2 configurado com sucesso");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao inicializar Cloudflare R2");
                    _isConfigured = false;
                    throw new InvalidOperationException($"Erro ao conectar com Cloudflare R2: {ex.Message}", ex);
                }
            }
        }

        public async Task<string> UploadFileAsync(Stream stream, string fileName, string hash, string contentType)
        {
            EnsureClientInitialized();

            if (_s3Client == null)
            {
                throw new InvalidOperationException("Cliente R2 não inicializado");
            }

            try
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                
                // Definir o caminho completo no bucket (sem / no início)
                var s3Key = $"music/{hash}/{uniqueFileName}";

                // Carregar stream completo na memória (necessário para compatibilidade com R2)
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var putRequest = new PutObjectRequest
                {
                    BucketName = _config.BucketName,
                    Key = s3Key,  // Sem / no início
                    InputStream = memoryStream,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead,
                    UseChunkEncoding = false
                };

                var response = await _s3Client.PutObjectAsync(putRequest);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Construir URL pública
                    var fileUrl = $"{_config.PublicUrl}/{s3Key}";

                    _logger.LogInformation($"✅ Upload concluído: {fileName} -> {s3Key}");
                    return fileUrl;
                }

                throw new Exception($"Erro no upload: {response.HttpStatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao fazer upload: {fileName}");
                throw;
            }
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (!_isConfigured)
            {
                _logger.LogWarning("R2 não configurado, arquivo não será deletado");
                return;
            }

            EnsureClientInitialized();

            if (_s3Client == null)
                return;

            try
            {
                // Extrair o caminho completo da URL (ex: music/hash/arquivo.mp3)
                var uri = new Uri(fileUrl);
                var s3Key = uri.AbsolutePath.TrimStart('/');

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _config.BucketName,
                    Key = s3Key
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
                _logger.LogInformation($"✅ Arquivo deletado: {s3Key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar arquivo: {fileUrl}");
                throw;
            }
        }

        public async Task<bool> FileExistsAsync(string fileName)
        {
            if (!_isConfigured)
                return false;

            try
            {
                EnsureClientInitialized();

                if (_s3Client == null)
                    return false;

                var request = new GetObjectMetadataRequest
                {
                    BucketName = _config.BucketName,
                    Key = fileName
                };

                await _s3Client.GetObjectMetadataAsync(request);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao verificar arquivo: {fileName}");
                return false;
            }
        }
    }
}
