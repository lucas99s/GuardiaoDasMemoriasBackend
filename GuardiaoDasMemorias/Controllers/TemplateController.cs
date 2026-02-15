using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Repository.Queries.Template;
using GuardiaoDasMemorias.Repository.Commands.Template;
using GuardiaoDasMemorias.Entities.Template;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateQueries _templateQueries;
        private readonly ITemplateCommands _templateCommands;

        public TemplateController(ITemplateQueries templateQueries, ITemplateCommands templateCommands)
        {
            _templateQueries = templateQueries;
            _templateCommands = templateCommands;
        }

        /// <summary>
        /// Obtém todos os templates
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var templates = await _templateQueries.GetAllAsync();
            return Ok(templates);
        }

        /// <summary>
        /// Obtém um template específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var template = await _templateQueries.GetByIdAsync(id);
            if (template == null)
            {
                return NotFound(new { message = "Template não encontrado" });
            }
            return Ok(template);
        }

        /// <summary>
        /// Cria um novo template
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Templates template)
        {
            var id = await _templateCommands.CreateAsync(template);
            var templateCreated = await _templateQueries.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, templateCreated);
        }

        /// <summary>
        /// Atualiza um template existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Templates templateAtualizado)
        {
            var templateExistente = await _templateQueries.GetByIdAsync(id);
            if (templateExistente == null)
            {
                return NotFound(new { message = "Template não encontrado" });
            }

            templateAtualizado.Id = id;
            await _templateCommands.UpdateAsync(templateAtualizado);

            var templateAtualiz = await _templateQueries.GetByIdAsync(id);
            return Ok(templateAtualiz);
        }

        /// <summary>
        /// Exclui um template
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var template = await _templateQueries.GetByIdAsync(id);
            if (template == null)
            {
                return NotFound(new { message = "Template não encontrado" });
            }

            await _templateCommands.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Obtém todos os templates de um tema específico
        /// </summary>
        [HttpGet("tema/{temaId}")]
        public async Task<ActionResult> GetByTema(int temaId)
        {
            var templates = await _templateQueries.GetByTemaIdAsync(temaId);
            return Ok(templates);
        }

        /// <summary>
        /// Obtém apenas templates ativos
        /// </summary>
        [HttpGet("ativos")]
        public async Task<ActionResult> GetAtivos()
        {
            var templates = await _templateQueries.GetAtivosAsync();
            return Ok(templates);
        }

        /// <summary>
        /// Ativa ou desativa um template
        /// </summary>
        [HttpPatch("{id}/toggle-ativo")]
        public async Task<ActionResult> ToggleAtivo(int id)
        {
            var template = await _templateQueries.GetByIdAsync(id);
            if (template == null)
            {
                return NotFound(new { message = "Template não encontrado" });
            }

            await _templateCommands.ToggleAtivoAsync(id);
            
            var templateAtualizado = await _templateQueries.GetByIdAsync(id);
            return Ok(templateAtualizado);
        }
    }
}
