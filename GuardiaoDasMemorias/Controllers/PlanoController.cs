using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GuardiaoDasMemorias.Entities.Plano;
using GuardiaoDasMemorias.Repository.Queries.Planos;
using GuardiaoDasMemorias.Repository.Commands.Planos;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoController : ControllerBase
    {
        private readonly IPlanoQueries _planoQueries;
        private readonly IPlanoCommands _planosCommands;
        private readonly ILogger<PlanoController> _logger;

        public PlanoController(IPlanoQueries planoQueries, IPlanoCommands planosCommands, ILogger<PlanoController> logger)
        {
            _planoQueries = planoQueries;
            _planosCommands = planosCommands;
            _logger = logger;
        }

        #region Planos - CRUD Básico

        /// <summary>
        /// Obtém todos os planos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var planos = await _planoQueries.GetAllAsync();
                return Ok(planos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os planos");
                return StatusCode(500, new { message = "Erro ao obter planos" });
            }
        }

        /// <summary>
        /// Obtém apenas os planos ativos
        /// </summary>
        [HttpGet("ativos")]
        public async Task<ActionResult> GetAtivos()
        {
            try
            {
                var planos = await _planoQueries.GetActiveAsync();
                return Ok(planos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter planos ativos");
                return StatusCode(500, new { message = "Erro ao obter planos ativos" });
            }
        }

        /// <summary>
        /// Obtém um plano específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var plano = await _planoQueries.GetByIdAsync(id);
                if (plano == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }
                return Ok(plano);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter plano por ID");
                return StatusCode(500, new { message = "Erro ao obter plano" });
            }
        }

        /// <summary>
        /// Obtém um plano por código
        /// </summary>
        [HttpGet("codigo/{code}")]
        public async Task<ActionResult> GetByCode(string code)
        {
            try
            {
                var plano = await _planoQueries.GetByCodeAsync(code);
                if (plano == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }
                return Ok(plano);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter plano por código");
                return StatusCode(500, new { message = "Erro ao obter plano" });
            }
        }

        /// <summary>
        /// Obtém um plano completo com limites e recursos
        /// </summary>
        [HttpGet("{id}/detalhes")]
        public async Task<ActionResult> GetComDetalhes(int id)
        {
            try
            {
                var plano = await _planoQueries.GetComDetalhesAsync(id);
                if (plano == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }
                return Ok(plano);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter plano com detalhes");
                return StatusCode(500, new { message = "Erro ao obter plano" });
            }
        }

        /// <summary>
        /// Cria um novo plano
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] Planos plano)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var id = await _planosCommands.CreateAsync(plano);
                var planoCreated = await _planoQueries.GetByIdAsync(id);
                
                _logger.LogInformation("Plano criado com sucesso. ID: {PlanoId}", id);
                return CreatedAtAction(nameof(GetById), new { id }, planoCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar plano");
                return StatusCode(500, new { message = "Erro ao criar plano" });
            }
        }

        /// <summary>
        /// Atualiza um plano existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] Planos planoAtualizado)
        {
            try
            {
                var planoExistente = await _planoQueries.GetByIdAsync(id);
                if (planoExistente == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                planoAtualizado.Id = id;
                var sucesso = await _planosCommands.UpdateAsync(planoAtualizado);

                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao atualizar plano" });
                }

                var planoAtualiz = await _planoQueries.GetByIdAsync(id);
                _logger.LogInformation("Plano atualizado com sucesso. ID: {PlanoId}", id);
                return Ok(planoAtualiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar plano");
                return StatusCode(500, new { message = "Erro ao atualizar plano" });
            }
        }

        /// <summary>
        /// Desativa um plano
        /// </summary>
        [HttpPatch("{id}/desativar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Deactivate(int id)
        {
            try
            {
                var plano = await _planoQueries.GetByIdAsync(id);
                if (plano == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }

                var sucesso = await _planosCommands.DeactivateAsync(id);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao desativar plano" });
                }

                _logger.LogInformation("Plano desativado. ID: {PlanoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desativar plano");
                return StatusCode(500, new { message = "Erro ao desativar plano" });
            }
        }

        /// <summary>
        /// Ativa um plano
        /// </summary>
        [HttpPatch("{id}/ativar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Activate(int id)
        {
            try
            {
                var plano = await _planoQueries.GetByIdAsync(id);
                if (plano == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }

                var sucesso = await _planosCommands.ActivateAsync(id);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao ativar plano" });
                }

                _logger.LogInformation("Plano ativado. ID: {PlanoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao ativar plano");
                return StatusCode(500, new { message = "Erro ao ativar plano" });
            }
        }

        /// <summary>
        /// Deleta um plano completamente
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var plano = await _planoQueries.GetByIdAsync(id);
                if (plano == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }

                var sucesso = await _planosCommands.DeleteAsync(id);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao deletar plano" });
                }

                _logger.LogInformation("Plano deletado. ID: {PlanoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar plano");
                return StatusCode(500, new { message = "Erro ao deletar plano" });
            }
        }

        #endregion

        #region Planos - Filtros

        /// <summary>
        /// Obtém planos de um tema
        /// </summary>
        [HttpGet("tema/{temaId}")]
        public async Task<ActionResult> GetByTema(int temaId)
        {
            try
            {
                var planos = await _planoQueries.GetByTemaAsync(temaId);
                return Ok(planos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter planos por tema");
                return StatusCode(500, new { message = "Erro ao obter planos" });
            }
        }

        /// <summary>
        /// Obtém planos ativos de um tema
        /// </summary>
        [HttpGet("tema/{temaId}/ativos")]
        public async Task<ActionResult> GetByTemaAtivos(int temaId)
        {
            try
            {
                var planos = await _planoQueries.GetByTemaActiveAsync(temaId);
                return Ok(planos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter planos ativos por tema");
                return StatusCode(500, new { message = "Erro ao obter planos" });
            }
        }

        /// <summary>
        /// Obtém planos por tipo de pagamento
        /// </summary>
        [HttpGet("tipo-pagamento/{tipoPagamentoId}")]
        public async Task<ActionResult> GetByTipoPagamento(int tipoPagamentoId)
        {
            try
            {
                var planos = await _planoQueries.GetByTipoPagamentoAsync(tipoPagamentoId);
                return Ok(planos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter planos por tipo de pagamento");
                return StatusCode(500, new { message = "Erro ao obter planos" });
            }
        }

        #endregion

        #region Limites

        /// <summary>
        /// Obtém limites de um plano
        /// </summary>
        [HttpGet("{planoId}/limites")]
        public async Task<ActionResult> GetLimites(int planoId)
        {
            try
            {
                var limites = await _planoQueries.GetLimitesAsync(planoId);
                return Ok(limites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter limites");
                return StatusCode(500, new { message = "Erro ao obter limites" });
            }
        }

        /// <summary>
        /// Adiciona um limite a um plano
        /// </summary>
        [HttpPost("{planoId}/limites")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddLimite(int planoId, [FromBody] PlanoLimites limite)
        {
            try
            {
                var plano = await _planoQueries.GetByIdAsync(planoId);
                if (plano == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                limite.PlanoId = planoId;
                var sucesso = await _planosCommands.AddLimiteAsync(limite);

                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao adicionar limite" });
                }

                _logger.LogInformation("Limite adicionado ao plano. PlanoID: {PlanoId}", planoId);
                return CreatedAtAction(nameof(GetLimites), new { planoId }, limite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar limite");
                return StatusCode(500, new { message = "Erro ao adicionar limite" });
            }
        }

        /// <summary>
        /// Atualiza um limite de um plano
        /// </summary>
        [HttpPut("limites/{limiteId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateLimite(int limiteId, [FromBody] PlanoLimites limite)
        {
            try
            {
                var limiteExistente = await _planoQueries.GetLimiteByIdAsync(limiteId);
                if (limiteExistente == null)
                {
                    return NotFound(new { message = "Limite não encontrado" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                limite.Id = limiteId;
                var sucesso = await _planosCommands.UpdateLimiteAsync(limite);

                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao atualizar limite" });
                }

                _logger.LogInformation("Limite atualizado. LimiteID: {LimiteId}", limiteId);
                return Ok(limite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar limite");
                return StatusCode(500, new { message = "Erro ao atualizar limite" });
            }
        }

        /// <summary>
        /// Remove um limite de um plano
        /// </summary>
        [HttpDelete("limites/{limiteId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveLimite(int limiteId)
        {
            try
            {
                var limite = await _planoQueries.GetLimiteByIdAsync(limiteId);
                if (limite == null)
                {
                    return NotFound(new { message = "Limite não encontrado" });
                }

                var sucesso = await _planosCommands.RemoveLimiteAsync(limiteId);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao remover limite" });
                }

                _logger.LogInformation("Limite removido. LimiteID: {LimiteId}", limiteId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover limite");
                return StatusCode(500, new { message = "Erro ao remover limite" });
            }
        }

        #endregion

        #region Recursos

        /// <summary>
        /// Obtém recursos de um plano
        /// </summary>
        [HttpGet("{planoId}/recursos")]
        public async Task<ActionResult> GetRecursos(int planoId)
        {
            try
            {
                var recursos = await _planoQueries.GetRecursosAsync(planoId);
                return Ok(recursos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter recursos");
                return StatusCode(500, new { message = "Erro ao obter recursos" });
            }
        }

        /// <summary>
        /// Obtém apenas recursos ativos de um plano
        /// </summary>
        [HttpGet("{planoId}/recursos/ativos")]
        public async Task<ActionResult> GetRecursosAtivos(int planoId)
        {
            try
            {
                var recursos = await _planoQueries.GetRecursosAtivosAsync(planoId);
                return Ok(recursos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter recursos ativos");
                return StatusCode(500, new { message = "Erro ao obter recursos ativos" });
            }
        }

        /// <summary>
        /// Adiciona um recurso a um plano
        /// </summary>
        [HttpPost("{planoId}/recursos")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddRecurso(int planoId, [FromBody] PlanoRecursos recurso)
        {
            try
            {
                var plano = await _planoQueries.GetByIdAsync(planoId);
                if (plano == null)
                {
                    return NotFound(new { message = "Plano não encontrado" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                recurso.PlanoId = planoId;
                var sucesso = await _planosCommands.AddRecursoAsync(recurso);

                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao adicionar recurso" });
                }

                _logger.LogInformation("Recurso adicionado ao plano. PlanoID: {PlanoId}", planoId);
                return CreatedAtAction(nameof(GetRecursos), new { planoId }, recurso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar recurso");
                return StatusCode(500, new { message = "Erro ao adicionar recurso" });
            }
        }

        /// <summary>
        /// Atualiza um recurso de um plano
        /// </summary>
        [HttpPut("recursos/{recursoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateRecurso(int recursoId, [FromBody] PlanoRecursos recurso)
        {
            try
            {
                var recursoExistente = await _planoQueries.GetRecursoByIdAsync(recursoId);
                if (recursoExistente == null)
                {
                    return NotFound(new { message = "Recurso não encontrado" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                recurso.Id = recursoId;
                var sucesso = await _planosCommands.UpdateRecursoAsync(recurso);

                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao atualizar recurso" });
                }

                _logger.LogInformation("Recurso atualizado. RecursoID: {RecursoId}", recursoId);
                return Ok(recurso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar recurso");
                return StatusCode(500, new { message = "Erro ao atualizar recurso" });
            }
        }

        /// <summary>
        /// Remove um recurso de um plano
        /// </summary>
        [HttpDelete("recursos/{recursoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveRecurso(int recursoId)
        {
            try
            {
                var recurso = await _planoQueries.GetRecursoByIdAsync(recursoId);
                if (recurso == null)
                {
                    return NotFound(new { message = "Recurso não encontrado" });
                }

                var sucesso = await _planosCommands.RemoveRecursoAsync(recursoId);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao remover recurso" });
                }

                _logger.LogInformation("Recurso removido. RecursoID: {RecursoId}", recursoId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover recurso");
                return StatusCode(500, new { message = "Erro ao remover recurso" });
            }
        }

        #endregion
    }
}
