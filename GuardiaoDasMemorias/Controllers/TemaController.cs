using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;
using GuardiaoDasMemorias.Repository.Queries.Tema;
using GuardiaoDasMemorias.Repository.Commands.Tema;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemaController : ControllerBase
    {
        private readonly ITemaQueries _temaQueries;
        private readonly ITemaCommands _temaCommands;

        public TemaController(ITemaQueries temaQueries, ITemaCommands temaCommands)
        {
            _temaQueries = temaQueries;
            _temaCommands = temaCommands;
        }

        /// <summary>
        /// Obtém todos os temas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var temas = await _temaQueries.GetAllAsync();
            return Ok(temas);
        }

        /// <summary>
        /// Obtém um tema específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var tema = await _temaQueries.GetByIdAsync(id);
            if (tema == null)
            {
                return NotFound(new { message = "Tema não encontrado" });
            }
            return Ok(tema);
        }

        /// <summary>
        /// Cria um novo tema
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Tema tema)
        {
            var id = await _temaCommands.CreateAsync(tema);
            var temaCreated = await _temaQueries.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, temaCreated);
        }

        /// <summary>
        /// Atualiza um tema existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Tema temaAtualizado)
        {
            var temaExistente = await _temaQueries.GetByIdAsync(id);
            if (temaExistente == null)
            {
                return NotFound(new { message = "Tema não encontrado" });
            }

            temaAtualizado.Id = id;
            await _temaCommands.UpdateAsync(temaAtualizado);

            var temaAtualiz = await _temaQueries.GetByIdAsync(id);
            return Ok(temaAtualiz);
        }

        /// <summary>
        /// Exclui um tema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var tema = await _temaQueries.GetByIdAsync(id);
            if (tema == null)
            {
                return NotFound(new { message = "Tema não encontrado" });
            }

            await _temaCommands.DeleteAsync(id);
            return NoContent();
        }
    }
}
