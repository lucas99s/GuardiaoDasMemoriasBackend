namespace GuardiaoDasMemorias.Services.CloudflareR2
{
    public interface ICloudflareR2Service
    {
        /// <summary>
        /// Faz upload de um arquivo para o Cloudflare R2
        /// </summary>
        /// <param name="stream">Stream do arquivo</param>
        /// <param name="fileName">Nome do arquivo</param>
        /// <param name="contentType">Tipo de conteúdo (ex: audio/mpeg)</param>
        /// <returns>URL pública do arquivo</returns>
        Task<string> UploadFileAsync(Stream stream, string fileName, string contentType);

        /// <summary>
        /// Deleta um arquivo do Cloudflare R2
        /// </summary>
        /// <param name="fileUrl">URL ou chave do arquivo</param>
        Task DeleteFileAsync(string fileUrl);

        /// <summary>
        /// Verifica se um arquivo existe no R2
        /// </summary>
        /// <param name="fileName">Nome do arquivo</param>
        Task<bool> FileExistsAsync(string fileName);
    }
}
