using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities.Memoria;
using GuardiaoDasMemorias.DTOs.Memoria;
using GuardiaoDasMemorias.Repository.Queries.Memoria;
using GuardiaoDasMemorias.Repository.Commands.Memoria;
using Microsoft.AspNetCore.Authorization;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemoriaController : ControllerBase
    {
        private readonly IMemoriaQueries _memoriaQueries;
        private readonly IMemoriaCommands _memoriaCommands;

        public MemoriaController(IMemoriaQueries memoriaQueries, IMemoriaCommands memoriaCommands)
        {
            _memoriaQueries = memoriaQueries;
            _memoriaCommands = memoriaCommands;
        }

        /// <summary>
        /// Obtém todas as memórias
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAll()
        {
            var memorias = await _memoriaQueries.GetAllAsync();
            return Ok(memorias);
        }

        /// <summary>
        /// Obtém uma memória específica por ID
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetById(int id)
        {
            var memoria = await _memoriaQueries.GetByIdAsync(id);
            if (memoria == null)
            {
                return NotFound(new { message = "Memória não encontrada" });
            }
            return Ok(memoria);
        }

        /// <summary>
        /// Obtém uma memória específica por hash único
        /// </summary>
        [HttpGet("hash/{hash}")]
        public async Task<ActionResult> GetByHash(string hash)
        {
            var memoria = await _memoriaQueries.GetByHashAsync(hash);
            if (memoria == null)
            {
                return NotFound(new { message = "Memória não encontrada" });
            }
            return Ok(memoria);
        }

        /// <summary>
        /// Cria uma nova memória
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] MemoriaDto memoriaDto)
        {
            var memoria = new Memorias
            {
                TemaId = memoriaDto.TemaId,
                TemplateId = memoriaDto.TemplateId,
                ClienteId = memoriaDto.ClienteId,
                MemoriaHash = memoriaDto.MemoriaHash ?? string.Empty
            };

            var id = await _memoriaCommands.CreateAsync(memoria);
            var memoriaCreated = await _memoriaQueries.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, memoriaCreated);
        }

        /// <summary>
        /// Atualiza uma memória existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(int id, [FromBody] MemoriaDto memoriaDto)
        {
            var memoriaExistente = await _memoriaQueries.GetByIdAsync(id);
            if (memoriaExistente == null)
            {
                return NotFound(new { message = "Memória não encontrada" });
            }

            var memoriaAtualizada = new Memorias
            {
                Id = id,
                TemaId = memoriaDto.TemaId,
                TemplateId = memoriaDto.TemplateId,
                ClienteId = memoriaDto.ClienteId,
                MemoriaHash = memoriaDto.MemoriaHash ?? string.Empty
            };

            await _memoriaCommands.UpdateAsync(memoriaAtualizada);

            var memoriaAtualiz = await _memoriaQueries.GetByIdAsync(id);
            return Ok(memoriaAtualiz);
        }

        /// <summary>
        /// Exclui uma memória
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var memoria = await _memoriaQueries.GetByIdAsync(id);
            if (memoria == null)
            {
                return NotFound(new { message = "Memória não encontrada" });
            }

            await _memoriaCommands.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Obtém todas as memórias de um cliente específico
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        [Authorize]
        public async Task<ActionResult> GetByCliente(int clienteId)
        {
            var memorias = await _memoriaQueries.GetByClienteIdAsync(clienteId);
            return Ok(memorias);
        }

        /// <summary>
        /// Obtém todas as memórias de um tema específico
        /// </summary>
        [HttpGet("tema/{temaId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetByTema(int temaId)
        {
            var memorias = await _memoriaQueries.GetByTemaIdAsync(temaId);
            return Ok(memorias);
        }
    }
}
