using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;
using GuardiaoDasMemorias.Repository.Queries.Musica;
using GuardiaoDasMemorias.Repository.Commands.Musica;
using GuardiaoDasMemorias.Services.CloudflareR2;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicaController : ControllerBase
    {
        private readonly IMusicaQueries _musicaQueries;
        private readonly IMusicaCommands _musicaCommands;
        private readonly ICloudflareR2Service _r2Service;

        public MusicaController(
            IMusicaQueries musicaQueries, 
            IMusicaCommands musicaCommands,
            ICloudflareR2Service r2Service)
        {
            _musicaQueries = musicaQueries;
            _musicaCommands = musicaCommands;
            _r2Service = r2Service;
        }

        /// <summary>
        /// Obtém todas as músicas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var musicas = await _musicaQueries.GetAllAsync();
            return Ok(musicas);
        }

        /// <summary>
        /// Obtém uma música específica por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var musica = await _musicaQueries.GetByIdAsync(id);
            if (musica == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }
            return Ok(musica);
        }

        /// <summary>
        /// Cria uma nova música
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Musica musica)
        {
            var id = await _musicaCommands.CreateAsync(musica);
            var musicaCreated = await _musicaQueries.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, musicaCreated);
        }

        /// <summary>
        /// Atualiza uma música existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Musica musicaAtualizada)
        {
            var musicaExistente = await _musicaQueries.GetByIdAsync(id);
            if (musicaExistente == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }

            musicaAtualizada.Id = id;
            await _musicaCommands.UpdateAsync(musicaAtualizada);

            var musicaAtualiz = await _musicaQueries.GetByIdAsync(id);
            return Ok(musicaAtualiz);
        }

        /// <summary>
        /// Faz upload de uma música para o Cloudflare R2
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadMusica(IFormFile arquivo, string nome, int memoriaId, string hash)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest(new { message = "Nenhum arquivo foi enviado" });
            }

            // Validar tipo de arquivo (permitir apenas áudio)
            var allowedTypes = new[] { "audio/mpeg", "audio/mp3", "audio/wav", "audio/ogg", "audio/webm" };
            if (!allowedTypes.Contains(arquivo.ContentType.ToLower()))
            {
                return BadRequest(new { message = $"Tipo de arquivo não permitido. Tipos aceitos: {string.Join(", ", allowedTypes)}" });
            }

            // Validar tamanho do arquivo (máximo 50MB)
            const long maxFileSize = 50 * 1024 * 1024; // 50MB
            if (arquivo.Length > maxFileSize)
            {
                return BadRequest(new { message = "Arquivo muito grande. Tamanho máximo: 50MB" });
            }

            try
            {
                // Fazer upload para R2
                using var stream = arquivo.OpenReadStream();
                var fileUrl = await _r2Service.UploadFileAsync(stream, arquivo.FileName, hash, arquivo.ContentType);

                // Salvar registro no banco de dados
                var musica = new Musica
                {
                    Nome = nome,
                    Caminho = fileUrl,
                    MemoriaId = memoriaId
                };

                var id = await _musicaCommands.CreateAsync(musica);
                var musicaCreated = await _musicaQueries.GetByIdAsync(id);

                return CreatedAtAction(nameof(GetById), new { id }, musicaCreated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao fazer upload: {ex.Message}" });
            }
        }

        /// <summary>
        /// Exclui uma música e remove do R2
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var musica = await _musicaQueries.GetByIdAsync(id);
            if (musica == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }

            try
            {
                // Deletar arquivo do R2
                if (!string.IsNullOrEmpty(musica.Caminho))
                {
                    await _r2Service.DeleteFileAsync(musica.Caminho);
                }

                // Deletar registro do banco
                await _musicaCommands.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao excluir música: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtém todas as músicas de uma memória específica
        /// </summary>
        [HttpGet("memoria/{memoriaHash}")]
        public async Task<ActionResult> GetByMemoria(string memoriaHash)
        {
            var musicas = await _musicaQueries.GetByMemoriaHashAsync(memoriaHash);
            return Ok(musicas);
        }

        /// <summary>
        /// Busca músicas por nome
        /// </summary>
        [HttpGet("buscar/{nome}")]
        public async Task<ActionResult> GetByNome(string nome)
        {
            var musicas = await _musicaQueries.GetByNomeAsync(nome);
            return Ok(musicas);
        }
    }
}
