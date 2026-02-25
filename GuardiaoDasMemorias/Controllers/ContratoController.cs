using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GuardiaoDasMemorias.Entities.Contrato;
using GuardiaoDasMemorias.Repository.Queries.Contratos;
using GuardiaoDasMemorias.Repository.Commands.Contratos;

namespace GuardiaoDasMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly IContratoQueries _contratoQueries;
        private readonly IContratoCommands _contratoCommands;
        private readonly ILogger<ContratoController> _logger;

        public ContratoController(IContratoQueries contratoQueries, IContratoCommands contratoCommands, ILogger<ContratoController> logger)
        {
            _contratoQueries = contratoQueries;
            _contratoCommands = contratoCommands;
            _logger = logger;
        }

        #region Contratos - CRUD Básico

        /// <summary>
        /// Obtém todos os contratos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var contratos = await _contratoQueries.GetAllAsync();
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os contratos");
                return StatusCode(500, new { message = "Erro ao obter contratos" });
            }
        }

        /// <summary>
        /// Obtém um contrato específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var contrato = await _contratoQueries.GetByIdAsync(id);
                if (contrato == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }
                return Ok(contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contrato por ID");
                return StatusCode(500, new { message = "Erro ao obter contrato" });
            }
        }

        /// <summary>
        /// Obtém um contrato com detalhes completos (memória, plano, status, origem, cliente)
        /// </summary>
        [HttpGet("{id}/detalhes")]
        public async Task<ActionResult> GetComDetalhes(int id)
        {
            try
            {
                var contrato = await _contratoQueries.GetComDetalhesAsync(id);
                if (contrato == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }
                return Ok(contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contrato com detalhes");
                return StatusCode(500, new { message = "Erro ao obter contrato" });
            }
        }

        /// <summary>
        /// Cria um novo contrato
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] ContratoMemoria contrato)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var id = await _contratoCommands.CreateAsync(contrato);
                var contratoCreated = await _contratoQueries.GetByIdAsync(id);
                
                _logger.LogInformation("Contrato criado com sucesso. ID: {ContratoId}", id);
                return CreatedAtAction(nameof(GetById), new { id }, contratoCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar contrato");
                return StatusCode(500, new { message = "Erro ao criar contrato" });
            }
        }

        /// <summary>
        /// Atualiza um contrato existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(int id, [FromBody] ContratoMemoria contratoAtualizado)
        {
            try
            {
                var contratoExistente = await _contratoQueries.GetByIdAsync(id);
                if (contratoExistente == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                contratoAtualizado.Id = id;
                var sucesso = await _contratoCommands.UpdateAsync(contratoAtualizado);

                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao atualizar contrato" });
                }

                var contratoAtualiz = await _contratoQueries.GetByIdAsync(id);
                _logger.LogInformation("Contrato atualizado com sucesso. ID: {ContratoId}", id);
                return Ok(contratoAtualiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar contrato");
                return StatusCode(500, new { message = "Erro ao atualizar contrato" });
            }
        }

        /// <summary>
        /// Deleta um contrato completamente
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var contrato = await _contratoQueries.GetByIdAsync(id);
                if (contrato == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }

                var sucesso = await _contratoCommands.DeleteAsync(id);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao deletar contrato" });
                }

                _logger.LogInformation("Contrato deletado. ID: {ContratoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar contrato");
                return StatusCode(500, new { message = "Erro ao deletar contrato" });
            }
        }

        #endregion

        #region Contratos - Por Status

        /// <summary>
        /// Obtém contratos por status específico
        /// </summary>
        [HttpGet("status/{statusId}")]
        public async Task<ActionResult> GetByStatus(int statusId)
        {
            try
            {
                var contratos = await _contratoQueries.GetByStatusAsync(statusId);
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos por status");
                return StatusCode(500, new { message = "Erro ao obter contratos" });
            }
        }

        /// <summary>
        /// Obtém apenas contratos PENDENTES (status_id = 1)
        /// </summary>
        [HttpGet("pendentes")]
        public async Task<ActionResult> GetPendentes()
        {
            try
            {
                var contratos = await _contratoQueries.GetPendentesAsync();
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos pendentes");
                return StatusCode(500, new { message = "Erro ao obter contratos pendentes" });
            }
        }

        /// <summary>
        /// Obtém apenas contratos ATIVOS (status_id = 2)
        /// </summary>
        [HttpGet("ativos")]
        public async Task<ActionResult> GetAtivos()
        {
            try
            {
                var contratos = await _contratoQueries.GetAtivosAsync();
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos ativos");
                return StatusCode(500, new { message = "Erro ao obter contratos ativos" });
            }
        }

        /// <summary>
        /// Obtém apenas contratos CANCELADOS (status_id = 3)
        /// </summary>
        [HttpGet("cancelados")]
        public async Task<ActionResult> GetCancelados()
        {
            try
            {
                var contratos = await _contratoQueries.GetCanceladosAsync();
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos cancelados");
                return StatusCode(500, new { message = "Erro ao obter contratos cancelados" });
            }
        }

        /// <summary>
        /// Obtém apenas contratos EXPIRADOS (status_id = 4)
        /// </summary>
        [HttpGet("expirados")]
        public async Task<ActionResult> GetExpirados()
        {
            try
            {
                var contratos = await _contratoQueries.GetExpiradosAsync();
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos expirados");
                return StatusCode(500, new { message = "Erro ao obter contratos expirados" });
            }
        }

        #endregion

        #region Contratos - Filtros Avançados

        /// <summary>
        /// Obtém contratos de um cliente
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        [Authorize]
        public async Task<ActionResult> GetByCliente(int clienteId)
        {
            try
            {
                var contratos = await _contratoQueries.GetByClienteAsync(clienteId);
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos por cliente");
                return StatusCode(500, new { message = "Erro ao obter contratos" });
            }
        }

        /// <summary>
        /// Obtém contratos de um cliente com detalhes completos
        /// </summary>
        [HttpGet("cliente/{clienteId}/detalhes")]
        [Authorize]
        public async Task<ActionResult> GetComDetalhesCliente(int clienteId)
        {
            try
            {
                var contratos = await _contratoQueries.GetComDetalhesClienteAsync(clienteId);
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos do cliente com detalhes");
                return StatusCode(500, new { message = "Erro ao obter contratos" });
            }
        }

        /// <summary>
        /// Obtém contratos de uma memória
        /// </summary>
        [HttpGet("memoria/{memoriaId}")]
        public async Task<ActionResult> GetByMemoria(int memoriaId)
        {
            try
            {
                var contratos = await _contratoQueries.GetByMemoriaAsync(memoriaId);
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos por memória");
                return StatusCode(500, new { message = "Erro ao obter contratos" });
            }
        }

        /// <summary>
        /// Obtém o contrato ATIVO de uma memória
        /// </summary>
        [HttpGet("memoria/{memoriaId}/ativo")]
        public async Task<ActionResult> GetContratoAtivoByMemoria(int memoriaId)
        {
            try
            {
                var contrato = await _contratoQueries.GetContratoAtivoByMemoriaAsync(memoriaId);
                if (contrato == null)
                {
                    return NotFound(new { message = "Nenhum contrato ativo encontrado para esta memória" });
                }
                return Ok(contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contrato ativo da memória");
                return StatusCode(500, new { message = "Erro ao obter contrato" });
            }
        }

        /// <summary>
        /// Obtém contratos próximos de expirar (com assinatura)
        /// </summary>
        [HttpGet("proximos-a-expirar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetProxiamosAExpirar([FromQuery] int diasAntes = 7)
        {
            try
            {
                var contratos = await _contratoQueries.GetProxiamosAExpirarAsync(diasAntes);
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contratos próximos de expirar");
                return StatusCode(500, new { message = "Erro ao obter contratos" });
            }
        }

        /// <summary>
        /// Obtém contrato por TransacaoId (busca global)
        /// </summary>
        [HttpGet("transacao/{transacaoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetByTransacao(string transacaoId)
        {
            try
            {
                var contratos = await _contratoQueries.GetByTransacaoAsync(transacaoId);
                if (!contratos.Any())
                {
                    return NotFound(new { message = "Nenhum contrato encontrado com esta transação" });
                }
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contrato por transação");
                return StatusCode(500, new { message = "Erro ao obter contrato" });
            }
        }

        #endregion

        #region Contratos - Mudanças de Status

        /// <summary>
        /// Marca um contrato como pago
        /// </summary>
        [HttpPatch("{id}/marcar-como-pago")]
        [Authorize]
        public async Task<ActionResult> MarkAsPaid(int id, [FromBody] MarkAsPaidRequest request)
        {
            try
            {
                var contrato = await _contratoQueries.GetByIdAsync(id);
                if (contrato == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }

                var sucesso = await _contratoCommands.MarkAsPaidAsync(id, request.TransacaoId);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao marcar contrato como pago" });
                }

                _logger.LogInformation("Contrato marcado como pago. ID: {ContratoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao marcar contrato como pago");
                return StatusCode(500, new { message = "Erro ao marcar contrato como pago" });
            }
        }

        /// <summary>
        /// Marca um contrato como cancelado
        /// </summary>
        [HttpPatch("{id}/marcar-como-cancelado")]
        [Authorize]
        public async Task<ActionResult> MarkAsCanceled(int id)
        {
            try
            {
                var contrato = await _contratoQueries.GetByIdAsync(id);
                if (contrato == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }

                var sucesso = await _contratoCommands.MarkAsCanceledAsync(id);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao marcar contrato como cancelado" });
                }

                _logger.LogInformation("Contrato marcado como cancelado. ID: {ContratoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao marcar contrato como cancelado");
                return StatusCode(500, new { message = "Erro ao marcar contrato como cancelado" });
            }
        }

        /// <summary>
        /// Marca um contrato como expirado
        /// </summary>
        [HttpPatch("{id}/marcar-como-expirado")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> MarkAsExpired(int id)
        {
            try
            {
                var contrato = await _contratoQueries.GetByIdAsync(id);
                if (contrato == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }

                var sucesso = await _contratoCommands.MarkAsExpiredAsync(id);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao marcar contrato como expirado" });
                }

                _logger.LogInformation("Contrato marcado como expirado. ID: {ContratoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao marcar contrato como expirado");
                return StatusCode(500, new { message = "Erro ao marcar contrato como expirado" });
            }
        }

        /// <summary>
        /// Altera o status de um contrato
        /// </summary>
        [HttpPatch("{id}/alterar-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeStatus(int id, [FromBody] ChangeStatusRequest request)
        {
            try
            {
                var contrato = await _contratoQueries.GetByIdAsync(id);
                if (contrato == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }

                var sucesso = await _contratoCommands.ChangeStatusAsync(id, request.NovoStatusId);
                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao alterar status do contrato" });
                }

                _logger.LogInformation("Status do contrato alterado. ID: {ContratoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status do contrato");
                return StatusCode(500, new { message = "Erro ao alterar status do contrato" });
            }
        }

        #endregion

        #region Contratos - Histórico

        /// <summary>
        /// Obtém o histórico completo de mudanças de um contrato
        /// </summary>
        [HttpGet("{id}/historico")]
        [Authorize]
        public async Task<ActionResult> GetHistorico(int id)
        {
            try
            {
                var contrato = await _contratoQueries.GetByIdAsync(id);
                if (contrato == null)
                {
                    return NotFound(new { message = "Contrato não encontrado" });
                }

                var historico = await _contratoQueries.GetHistoricoAsync(id);
                return Ok(historico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter histórico do contrato");
                return StatusCode(500, new { message = "Erro ao obter histórico" });
            }
        }

        /// <summary>
        /// Obtém histórico onde um contrato é a ORIGEM (foi substituído)
        /// </summary>
        [HttpGet("{id}/historico/origem")]
        [Authorize]
        public async Task<ActionResult> GetHistoricoOrigem(int id)
        {
            try
            {
                var historico = await _contratoQueries.GetHistoricoOrigemAsync(id);
                if (historico == null)
                {
                    return NotFound(new { message = "Nenhum histórico de origem encontrado" });
                }
                return Ok(historico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter histórico de origem");
                return StatusCode(500, new { message = "Erro ao obter histórico" });
            }
        }

        /// <summary>
        /// Obtém histórico onde um contrato é o DESTINO (substituiu outro)
        /// </summary>
        [HttpGet("{id}/historico/destino")]
        [Authorize]
        public async Task<ActionResult> GetHistoricoDestino(int id)
        {
            try
            {
                var historico = await _contratoQueries.GetHistoricoDestinoAsync(id);
                if (historico == null)
                {
                    return NotFound(new { message = "Nenhum histórico de destino encontrado" });
                }
                return Ok(historico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter histórico de destino");
                return StatusCode(500, new { message = "Erro ao obter histórico" });
            }
        }

        /// <summary>
        /// Obtém históricos por tipo de mudança (Upgrade, Downgrade, Renovação)
        /// </summary>
        [HttpGet("historico/tipo/{tipoMudanca}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetHistoricoByTipo(string tipoMudanca)
        {
            try
            {
                var historicos = await _contratoQueries.GetHistoricoByTipoAsync(tipoMudanca);
                return Ok(historicos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter histórico por tipo de mudança");
                return StatusCode(500, new { message = "Erro ao obter histórico" });
            }
        }

        /// <summary>
        /// Processa um upgrade/downgrade de plano (cria histórico)
        /// </summary>
        [HttpPost("{id}/processar-upgrade")]
        [Authorize]
        public async Task<ActionResult> ProcessUpgrade(int id, [FromBody] ProcessUpgradeRequest request)
        {
            try
            {
                var contratoAnterior = await _contratoQueries.GetByIdAsync(id);
                if (contratoAnterior == null)
                {
                    return NotFound(new { message = "Contrato anterior não encontrado" });
                }

                var contratoNovo = await _contratoQueries.GetByIdAsync(request.ContratoNovoId);
                if (contratoNovo == null)
                {
                    return NotFound(new { message = "Contrato novo não encontrado" });
                }

                var sucesso = await _contratoCommands.ProcessUpgradeAsync(
                    id,
                    request.ContratoNovoId,
                    request.TipoMudanca,
                    request.Observacao
                );

                if (!sucesso)
                {
                    return BadRequest(new { message = "Falha ao processar upgrade" });
                }

                _logger.LogInformation("Upgrade processado. Contrato anterior: {ContratoId1}, Contrato novo: {ContratoId2}", id, request.ContratoNovoId);
                return Ok(new { message = "Upgrade processado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar upgrade");
                return StatusCode(500, new { message = "Erro ao processar upgrade" });
            }
        }

        #endregion
    }

    #region DTOs de Request

    /// <summary>
    /// DTO para marcar contrato como pago
    /// </summary>
    public class MarkAsPaidRequest
    {
        public string? TransacaoId { get; set; }
    }

    /// <summary>
    /// DTO para alterar status do contrato
    /// </summary>
    public class ChangeStatusRequest
    {
        public int NovoStatusId { get; set; }
    }

    /// <summary>
    /// DTO para processar upgrade/downgrade de plano
    /// </summary>
    public class ProcessUpgradeRequest
    {
        public int ContratoNovoId { get; set; }
        public string TipoMudanca { get; set; } = "Upgrade"; // Upgrade, Downgrade, Renovação
        public string? Observacao { get; set; }
    }

    #endregion
}
