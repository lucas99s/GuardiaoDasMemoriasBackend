using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;
using GuardiaoDasMemorias.Repository.Queries.Musica;
using GuardiaoDasMemorias.Repository.Commands.Musica;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicaController : ControllerBase
    {
        private readonly IMusicaQueries _musicaQueries;
        private readonly IMusicaCommands _musicaCommands;

        public MusicaController(IMusicaQueries musicaQueries, IMusicaCommands musicaCommands)
        {
            _musicaQueries = musicaQueries;
            _musicaCommands = musicaCommands;
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
        /// Exclui uma música
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var musica = await _musicaQueries.GetByIdAsync(id);
            if (musica == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }

            await _musicaCommands.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Obtém todas as músicas de um cliente específico
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult> GetByCliente(int clienteId)
        {
            var musicas = await _musicaQueries.GetByClienteIdAsync(clienteId);
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
