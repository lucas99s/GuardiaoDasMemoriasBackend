using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicaController : ControllerBase
    {
        private static readonly List<Musica> _musicas = new()
        {
            new Musica
            {
                Id = 1,
                Nome = "Somewhere Over The Rainbow",
                Caminho = "/musicas/somewhere-over-rainbow.mp3",
                ClienteId = 1
            },
            new Musica
            {
                Id = 2,
                Nome = "What a Wonderful World",
                Caminho = "/musicas/wonderful-world.mp3",
                ClienteId = 1
            },
            new Musica
            {
                Id = 3,
                Nome = "Stand By Me",
                Caminho = "/musicas/stand-by-me.mp3",
                ClienteId = 2
            }
        };

        /// <summary>
        /// Obtém todas as músicas
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Musica>> GetAll()
        {
            return Ok(_musicas);
        }

        /// <summary>
        /// Obtém uma música específica por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Musica> GetById(int id)
        {
            var musica = _musicas.FirstOrDefault(m => m.Id == id);
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
        public ActionResult<Musica> Create([FromBody] Musica musica)
        {
            musica.Id = _musicas.Any() ? _musicas.Max(m => m.Id) + 1 : 1;
            _musicas.Add(musica);
            return CreatedAtAction(nameof(GetById), new { id = musica.Id }, musica);
        }

        /// <summary>
        /// Atualiza uma música existente
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<Musica> Update(int id, [FromBody] Musica musicaAtualizada)
        {
            var musica = _musicas.FirstOrDefault(m => m.Id == id);
            if (musica == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }

            musica.Nome = musicaAtualizada.Nome;
            musica.Caminho = musicaAtualizada.Caminho;
            musica.ClienteId = musicaAtualizada.ClienteId;

            return Ok(musica);
        }

        /// <summary>
        /// Exclui uma música
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var musica = _musicas.FirstOrDefault(m => m.Id == id);
            if (musica == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }

            _musicas.Remove(musica);
            return NoContent();
        }

        /// <summary>
        /// Obtém todas as músicas de um cliente específico
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        public ActionResult<IEnumerable<Musica>> GetByCliente(int clienteId)
        {
            var musicas = _musicas.Where(m => m.ClienteId == clienteId);
            return Ok(musicas);
        }

        /// <summary>
        /// Busca músicas por nome
        /// </summary>
        [HttpGet("buscar/{nome}")]
        public ActionResult<IEnumerable<Musica>> GetByNome(string nome)
        {
            var musicas = _musicas.Where(m => m.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
            return Ok(musicas);
        }
    }
}
