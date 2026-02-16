using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuardiaoDasMemorias.Migrations
{
    /// <inheritdoc />
    public partial class CriaIndicesUnicosParciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1 contrato ATIVO por memória (status_id = 2)
            // Permite múltiplos contratos no histórico, mas apenas 1 pode estar ativo
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX ux_contrato_memoria_ativo_por_memoria
                ON pagamento.contrato_memoria (memoria_id)
                WHERE contrato_status_id = 2;
            ");

            // transacao_id único quando não nulo
            // Permite contratos pendentes sem transacao_id, mas garante que IDs de transação sejam únicos
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX ux_contrato_memoria_transacao_id_not_null
                ON pagamento.contrato_memoria (transacao_id)
                WHERE transacao_id IS NOT NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS pagamento.ux_contrato_memoria_ativo_por_memoria;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS pagamento.ux_contrato_memoria_transacao_id_not_null;");
        }
    }
}
