using GuardiaoDasMemorias.Entities.Cliente;
using GuardiaoDasMemorias.Entities.Memoria;

namespace GuardiaoDasMemorias.Entities.Pagamentos
{
    public class ContratoMemoria
    {
        public int Id { get; set; }
        public int MemoriaId { get; set; }
        public int PlanoId { get; set; }
        public int ContratoStatusId { get; set; }
        public int ContratoOrigemId { get; set; }
        public int ClienteId { get; set; } // Comprador (pode ser diferente do dono da memória em casos de presente)
        public decimal ValorPago { get; set; } // Valor que foi pago (importante guardar histórico)
        public string? TransacaoId { get; set; } // ID da transação do gateway (Stripe, Asaas, etc.)
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? PagoEm { get; set; } // Quando foi efetivamente pago
        public DateTime? ExpiraEm { get; set; } // Para assinaturas
        public DateTime? CanceladoEm { get; set; } // Se foi cancelado
        public required Memorias Memoria { get; set; }
        public required Planos Plano { get; set; }
        public required ContratoStatus ContratoStatus { get; set; }
        public required ContratoOrigem ContratoOrigem { get; set; }
        public required Clientes Cliente { get; set; }
        
        // Histórico de mudanças - quando este contrato foi substituído por outro
        public ICollection<ContratoHistorico> HistoricoComoAntigo { get; set; } = new List<ContratoHistorico>();
        
        // Histórico de mudanças - quando este contrato substituiu outro
        public ICollection<ContratoHistorico> HistoricoComoNovo { get; set; } = new List<ContratoHistorico>();
    }
}
