using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private static readonly List<Template> _templates = new()
        {
            new Template
            {
                Id = 1,
                TemaId = 1,
                TemplateId = 101,
                ClienteId = 1
            },
            new Template
            {
                Id = 2,
                TemaId = 2,
                TemplateId = 102,
                ClienteId = 1
            },
            new Template
            {
                Id = 3,
                TemaId = 3,
                TemplateId = 103,
                ClienteId = 2
            }
        };

        /// <summary>
        /// Obtém todos os templates
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Template>> GetAll()
        {
            return Ok(_templates);
        }

        /// <summary>
        /// Obtém um template específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Template> GetById(int id)
        {
            var template = _templates.FirstOrDefault(t => t.Id == id);
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
        public ActionResult<Template> Create([FromBody] Template template)
        {
            template.Id = _templates.Any() ? _templates.Max(t => t.Id) + 1 : 1;
            _templates.Add(template);
            return CreatedAtAction(nameof(GetById), new { id = template.Id }, template);
        }

        /// <summary>
        /// Atualiza um template existente
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<Template> Update(int id, [FromBody] Template templateAtualizado)
        {
            var template = _templates.FirstOrDefault(t => t.Id == id);
            if (template == null)
            {
                return NotFound(new { message = "Template não encontrado" });
            }

            template.TemaId = templateAtualizado.TemaId;
            template.TemplateId = templateAtualizado.TemplateId;
            template.ClienteId = templateAtualizado.ClienteId;

            return Ok(template);
        }

        /// <summary>
        /// Exclui um template
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var template = _templates.FirstOrDefault(t => t.Id == id);
            if (template == null)
            {
                return NotFound(new { message = "Template não encontrado" });
            }

            _templates.Remove(template);
            return NoContent();
        }

        /// <summary>
        /// Obtém todos os templates de um cliente específico
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        public ActionResult<IEnumerable<Template>> GetByCliente(int clienteId)
        {
            var templates = _templates.Where(t => t.ClienteId == clienteId);
            return Ok(templates);
        }

        /// <summary>
        /// Obtém todos os templates de um tema específico
        /// </summary>
        [HttpGet("tema/{temaId}")]
        public ActionResult<IEnumerable<Template>> GetByTema(int temaId)
        {
            var templates = _templates.Where(t => t.TemaId == temaId);
            return Ok(templates);
        }
    }
}
