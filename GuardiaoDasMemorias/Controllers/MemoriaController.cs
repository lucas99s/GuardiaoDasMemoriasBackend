using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemoriaController : ControllerBase
    {
        private static readonly List<Memoria> _memorias = new()
        {
            new Memoria
            {
                Id = 1,
                TemaId = 1,
                TemplateId = 101,
                ClienteId = 1
            },
            new Memoria
            {
                Id = 2,
                TemaId = 2,
                TemplateId = 102,
                ClienteId = 1
            },
            new Memoria
            {
                Id = 3,
                TemaId = 3,
                TemplateId = 103,
                ClienteId = 2
            }
        };

        /// <summary>
        /// Obtém todas as memórias
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Memoria>> GetAll()
        {
            return Ok(_memorias);
        }

        /// <summary>
        /// Obtém uma memória específica por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Memoria> GetById(int id)
        {
            var memoria = _memorias.FirstOrDefault(t => t.Id == id);
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
        public ActionResult<Memoria> Create([FromBody] Memoria memoria)
        {
            memoria.Id = _memorias.Any() ? _memorias.Max(t => t.Id) + 1 : 1;
            _memorias.Add(memoria);
            return CreatedAtAction(nameof(GetById), new { id = memoria.Id }, memoria);
        }

        /// <summary>
        /// Atualiza uma memória existente
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<Memoria> Update(int id, [FromBody] Memoria memoriaAtualizada)
        {
            var memoria = _memorias.FirstOrDefault(t => t.Id == id);
            if (memoria == null)
            {
                return NotFound(new { message = "Memória não encontrada" });
            }

            memoria.TemaId = memoriaAtualizada.TemaId;
            memoria.TemplateId = memoriaAtualizada.TemplateId;
            memoria.ClienteId = memoriaAtualizada.ClienteId;

            return Ok(memoria);
        }

        /// <summary>
        /// Exclui uma memória
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var memoria = _memorias.FirstOrDefault(t => t.Id == id);
            if (memoria == null)
            {
                return NotFound(new { message = "Memória não encontrada" });
            }

            _memorias.Remove(memoria);
            return NoContent();
        }

        /// <summary>
        /// Obtém todas as memórias de um cliente específico
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        public ActionResult<IEnumerable<Memoria>> GetByCliente(int clienteId)
        {
            var memorias = _memorias.Where(t => t.ClienteId == clienteId);
            return Ok(memorias);
        }

        /// <summary>
        /// Obtém todas as memórias de um tema específico
        /// </summary>
        [HttpGet("tema/{temaId}")]
        public ActionResult<IEnumerable<Memoria>> GetByTema(int temaId)
        {
            var memorias = _memorias.Where(t => t.TemaId == temaId);
            return Ok(memorias);
        }
    }
}
