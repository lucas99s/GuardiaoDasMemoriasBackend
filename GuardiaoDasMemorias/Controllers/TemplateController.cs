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
                Nome = "Template Romântico Clássico",
                Ativo = true,
                TemaId = 1
            },
            new Template
            {
                Id = 2,
                Nome = "Template Infantil Alegre",
                Ativo = true,
                TemaId = 2
            },
            new Template
            {
                Id = 3,
                Nome = "Template Corporativo Profissional",
                Ativo = true,
                TemaId = 3
            },
            new Template
            {
                Id = 4,
                Nome = "Template Festivo Colorido",
                Ativo = true,
                TemaId = 4
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

            template.Nome = templateAtualizado.Nome;
            template.Ativo = templateAtualizado.Ativo;
            template.TemaId = templateAtualizado.TemaId;

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
        /// Obtém todos os templates de um tema específico
        /// </summary>
        [HttpGet("tema/{temaId}")]
        public ActionResult<IEnumerable<Template>> GetByTema(int temaId)
        {
            var templates = _templates.Where(t => t.TemaId == temaId);
            return Ok(templates);
        }

        /// <summary>
        /// Obtém apenas templates ativos
        /// </summary>
        [HttpGet("ativos")]
        public ActionResult<IEnumerable<Template>> GetAtivos()
        {
            var templates = _templates.Where(t => t.Ativo);
            return Ok(templates);
        }

        /// <summary>
        /// Ativa ou desativa um template
        /// </summary>
        [HttpPatch("{id}/toggle-ativo")]
        public ActionResult<Template> ToggleAtivo(int id)
        {
            var template = _templates.FirstOrDefault(t => t.Id == id);
            if (template == null)
            {
                return NotFound(new { message = "Template não encontrado" });
            }

            template.Ativo = !template.Ativo;
            return Ok(template);
        }
    }
}
