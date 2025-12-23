using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.Entities;
using GuardiaoDasMemorias.Repository.Queries.Cliente;
using GuardiaoDasMemorias.Repository.Commands.Cliente;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteQueries _clienteQueries;
        private readonly IClienteCommands _clienteCommands;

        public ClienteController(IClienteQueries clienteQueries, IClienteCommands clienteCommands)
        {
            _clienteQueries = clienteQueries;
            _clienteCommands = clienteCommands;
        }

        /// <summary>
        /// Obtém todos os clientes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var clientes = await _clienteQueries.GetAllAsync();
            return Ok(clientes);
        }

        /// <summary>
        /// Obtém um cliente específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var cliente = await _clienteQueries.GetByIdAsync(id);
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
        public async Task<ActionResult> Create([FromBody] Cliente cliente)
        {
            var id = await _clienteCommands.CreateAsync(cliente);
            var clienteCreated = await _clienteQueries.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, clienteCreated);
        }

        /// <summary>
        /// Atualiza um cliente existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Cliente clienteAtualizado)
        {
            var clienteExistente = await _clienteQueries.GetByIdAsync(id);
            if (clienteExistente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }

            clienteAtualizado.Id = id;
            await _clienteCommands.UpdateAsync(clienteAtualizado);

            var clienteAtualiz = await _clienteQueries.GetByIdAsync(id);
            return Ok(clienteAtualiz);
        }

        /// <summary>
        /// Exclui um cliente
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _clienteQueries.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }

            await _clienteCommands.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Busca clientes por nome
        /// </summary>
        [HttpGet("buscar/{nome}")]
        public async Task<ActionResult> GetByNome(string nome)
        {
            var clientes = await _clienteQueries.GetByNomeAsync(nome);
            return Ok(clientes);
        }
    }
}
