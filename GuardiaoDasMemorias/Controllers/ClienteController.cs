using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private static readonly List<Cliente> _clientes = new()
        {
            new Cliente
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "(11) 98765-4321",
                Email = "joao.silva@email.com"
            },
            new Cliente
            {
                Id = 2,
                Nome = "Maria Santos",
                Telefone = "(21) 97654-3210",
                Email = "maria.santos@email.com"
            }
        };

        /// <summary>
        /// Obtém todos os clientes
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetAll()
        {
            return Ok(_clientes);
        }

        /// <summary>
        /// Obtém um cliente específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Cliente> GetById(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }
            return Ok(cliente);
        }

        /// <summary>
        /// Cria um novo cliente
        /// </summary>
        [HttpPost]
        public ActionResult<Cliente> Create([FromBody] Cliente cliente)
        {
            cliente.Id = _clientes.Any() ? _clientes.Max(c => c.Id) + 1 : 1;
            _clientes.Add(cliente);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        /// <summary>
        /// Atualiza um cliente existente
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<Cliente> Update(int id, [FromBody] Cliente clienteAtualizado)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }

            cliente.Nome = clienteAtualizado.Nome;
            cliente.Telefone = clienteAtualizado.Telefone;
            cliente.Email = clienteAtualizado.Email;

            return Ok(cliente);
        }

        /// <summary>
        /// Exclui um cliente
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }

            _clientes.Remove(cliente);
            return NoContent();
        }

        /// <summary>
        /// Busca clientes por nome
        /// </summary>
        [HttpGet("buscar/{nome}")]
        public ActionResult<IEnumerable<Cliente>> GetByNome(string nome)
        {
            var clientes = _clientes.Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
            return Ok(clientes);
        }
    }
}
