using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuardiaoDasMemorias.Migrations
{
    /// <inheritdoc />
    public partial class SeparaTabelasSchemaPagamentoParaPlanoOuContrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "contrato");

            migrationBuilder.EnsureSchema(
                name: "plano");

            migrationBuilder.RenameTable(
                name: "tipo_pagamento",
                schema: "pagamento",
                newName: "tipo_pagamento",
                newSchema: "plano");

            migrationBuilder.RenameTable(
                name: "planos",
                schema: "pagamento",
                newName: "planos",
                newSchema: "plano");

            migrationBuilder.RenameTable(
                name: "plano_recursos",
                schema: "pagamento",
                newName: "plano_recursos",
                newSchema: "plano");

            migrationBuilder.RenameTable(
                name: "plano_limites",
                schema: "pagamento",
                newName: "plano_limites",
                newSchema: "plano");

            migrationBuilder.RenameTable(
                name: "contrato_status",
                schema: "pagamento",
                newName: "contrato_status",
                newSchema: "contrato");

            migrationBuilder.RenameTable(
                name: "contrato_origem",
                schema: "pagamento",
                newName: "contrato_origem",
                newSchema: "contrato");

            migrationBuilder.RenameTable(
                name: "contrato_memoria",
                schema: "pagamento",
                newName: "contrato_memoria",
                newSchema: "contrato");

            migrationBuilder.RenameTable(
                name: "contrato_historico",
                schema: "pagamento",
                newName: "contrato_historico",
                newSchema: "contrato");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pagamento");

            migrationBuilder.RenameTable(
                name: "tipo_pagamento",
                schema: "plano",
                newName: "tipo_pagamento",
                newSchema: "pagamento");

            migrationBuilder.RenameTable(
                name: "planos",
                schema: "plano",
                newName: "planos",
                newSchema: "pagamento");

            migrationBuilder.RenameTable(
                name: "plano_recursos",
                schema: "plano",
                newName: "plano_recursos",
                newSchema: "pagamento");

            migrationBuilder.RenameTable(
                name: "plano_limites",
                schema: "plano",
                newName: "plano_limites",
                newSchema: "pagamento");

            migrationBuilder.RenameTable(
                name: "contrato_status",
                schema: "contrato",
                newName: "contrato_status",
                newSchema: "pagamento");

            migrationBuilder.RenameTable(
                name: "contrato_origem",
                schema: "contrato",
                newName: "contrato_origem",
                newSchema: "pagamento");

            migrationBuilder.RenameTable(
                name: "contrato_memoria",
                schema: "contrato",
                newName: "contrato_memoria",
                newSchema: "pagamento");

            migrationBuilder.RenameTable(
                name: "contrato_historico",
                schema: "contrato",
                newName: "contrato_historico",
                newSchema: "pagamento");
        }
    }
}
