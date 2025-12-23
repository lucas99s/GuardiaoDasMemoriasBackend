using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemaController : ControllerBase
    {
        private static readonly List<Tema> _temas = new()
        {
            new Tema
            {
                Id = 1,
                Nome = "Romântico"
            },
            new Tema
            {
                Id = 2,
                Nome = "Infantil"
            },
            new Tema
            {
                Id = 3,
                Nome = "Corporativo"
            },
            new Tema
            {
                Id = 4,
                Nome = "Festivo"
            }
        };

        /// <summary>
        /// Obtém todos os temas
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Tema>> GetAll()
        {
            return Ok(_temas);
        }

        /// <summary>
        /// Obtém um tema específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Tema> GetById(int id)
        {
            var tema = _temas.FirstOrDefault(t => t.Id == id);
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
        public ActionResult<Tema> Create([FromBody] Tema tema)
        {
            tema.Id = _temas.Any() ? _temas.Max(t => t.Id) + 1 : 1;
            _temas.Add(tema);
            return CreatedAtAction(nameof(GetById), new { id = tema.Id }, tema);
        }

        /// <summary>
        /// Atualiza um tema existente
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<Tema> Update(int id, [FromBody] Tema temaAtualizado)
        {
            var tema = _temas.FirstOrDefault(t => t.Id == id);
            if (tema == null)
            {
                return NotFound(new { message = "Tema não encontrado" });
            }

            tema.Nome = temaAtualizado.Nome;

            return Ok(tema);
        }

        /// <summary>
        /// Exclui um tema
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var tema = _temas.FirstOrDefault(t => t.Id == id);
            if (tema == null)
            {
                return NotFound(new { message = "Tema não encontrado" });
            }

            _temas.Remove(tema);
            return NoContent();
        }
    }
}
