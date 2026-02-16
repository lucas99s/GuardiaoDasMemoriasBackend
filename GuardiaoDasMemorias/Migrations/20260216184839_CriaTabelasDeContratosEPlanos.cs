using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GuardiaoDasMemorias.Migrations
{
    /// <inheritdoc />
    public partial class CriaTabelasDeContratosEPlanos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pagamento");

            migrationBuilder.CreateTable(
                name: "contrato_origem",
                schema: "pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contrato_origem", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contrato_status",
                schema: "pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contrato_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_pagamento",
                schema: "pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_pagamento", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "planos",
                schema: "pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    tema_id = table.Column<int>(type: "integer", nullable: false),
                    tipo_pagamento_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    preco = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ordem = table.Column<int>(type: "integer", nullable: false),
                    criado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    atualizado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_planos", x => x.id);
                    table.ForeignKey(
                        name: "FK_planos_temas_tema_id",
                        column: x => x.tema_id,
                        principalSchema: "tema",
                        principalTable: "temas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_planos_tipo_pagamento_tipo_pagamento_id",
                        column: x => x.tipo_pagamento_id,
                        principalSchema: "pagamento",
                        principalTable: "tipo_pagamento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contrato_memoria",
                schema: "pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    memoria_id = table.Column<int>(type: "integer", nullable: false),
                    plano_id = table.Column<int>(type: "integer", nullable: false),
                    contrato_status_id = table.Column<int>(type: "integer", nullable: false),
                    contrato_origem_id = table.Column<int>(type: "integer", nullable: false),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    valor_pago = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    transacao_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pago_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expira_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contrato_memoria", x => x.id);
                    table.ForeignKey(
                        name: "FK_contrato_memoria_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "cliente",
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contrato_memoria_contrato_origem_contrato_origem_id",
                        column: x => x.contrato_origem_id,
                        principalSchema: "pagamento",
                        principalTable: "contrato_origem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contrato_memoria_contrato_status_contrato_status_id",
                        column: x => x.contrato_status_id,
                        principalSchema: "pagamento",
                        principalTable: "contrato_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contrato_memoria_memorias_memoria_id",
                        column: x => x.memoria_id,
                        principalSchema: "memoria",
                        principalTable: "memorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contrato_memoria_planos_plano_id",
                        column: x => x.plano_id,
                        principalSchema: "pagamento",
                        principalTable: "planos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "plano_limites",
                schema: "pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    plano_id = table.Column<int>(type: "integer", nullable: false),
                    propriedade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    valor = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plano_limites", x => x.id);
                    table.ForeignKey(
                        name: "FK_plano_limites_planos_plano_id",
                        column: x => x.plano_id,
                        principalSchema: "pagamento",
                        principalTable: "planos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plano_recursos",
                schema: "pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    plano_id = table.Column<int>(type: "integer", nullable: false),
                    recurso_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ordem = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plano_recursos", x => x.id);
                    table.ForeignKey(
                        name: "FK_plano_recursos_planos_plano_id",
                        column: x => x.plano_id,
                        principalSchema: "pagamento",
                        principalTable: "planos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contrato_historico",
                schema: "pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    contrato_antigo_id = table.Column<int>(type: "integer", nullable: false),
                    contrato_novo_id = table.Column<int>(type: "integer", nullable: false),
                    tipo_mudanca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    observacao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    realizado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contrato_historico", x => x.id);
                    table.CheckConstraint("CK_ContratoHistorico_DiferentesContratos", "contrato_antigo_id != contrato_novo_id");
                    table.ForeignKey(
                        name: "FK_contrato_historico_contrato_memoria_contrato_antigo_id",
                        column: x => x.contrato_antigo_id,
                        principalSchema: "pagamento",
                        principalTable: "contrato_memoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contrato_historico_contrato_memoria_contrato_novo_id",
                        column: x => x.contrato_novo_id,
                        principalSchema: "pagamento",
                        principalTable: "contrato_memoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "pagamento",
                table: "contrato_origem",
                columns: new[] { "id", "nome" },
                values: new object[,]
                {
                    { 1, "Compra no Site" },
                    { 2, "Afiliado" },
                    { 3, "Presente Admin" }
                });

            migrationBuilder.InsertData(
                schema: "pagamento",
                table: "contrato_status",
                columns: new[] { "id", "nome" },
                values: new object[,]
                {
                    { 1, "Pendente" },
                    { 2, "Ativo" },
                    { 3, "Cancelado" },
                    { 4, "Expirado" }
                });

            migrationBuilder.InsertData(
                schema: "pagamento",
                table: "tipo_pagamento",
                columns: new[] { "id", "nome" },
                values: new object[,]
                {
                    { 1, "Pagamento Único" },
                    { 2, "Assinatura" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_contrato_historico_contrato_antigo_id",
                schema: "pagamento",
                table: "contrato_historico",
                column: "contrato_antigo_id");

            migrationBuilder.CreateIndex(
                name: "IX_contrato_historico_contrato_novo_id",
                schema: "pagamento",
                table: "contrato_historico",
                column: "contrato_novo_id");

            migrationBuilder.CreateIndex(
                name: "IX_contrato_historico_tipo_mudanca_realizado_em",
                schema: "pagamento",
                table: "contrato_historico",
                columns: new[] { "tipo_mudanca", "realizado_em" });

            migrationBuilder.CreateIndex(
                name: "IX_contrato_memoria_cliente_id",
                schema: "pagamento",
                table: "contrato_memoria",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_contrato_memoria_contrato_origem_id",
                schema: "pagamento",
                table: "contrato_memoria",
                column: "contrato_origem_id");

            migrationBuilder.CreateIndex(
                name: "IX_contrato_memoria_contrato_status_id_expira_em",
                schema: "pagamento",
                table: "contrato_memoria",
                columns: new[] { "contrato_status_id", "expira_em" });

            migrationBuilder.CreateIndex(
                name: "IX_contrato_memoria_memoria_id",
                schema: "pagamento",
                table: "contrato_memoria",
                column: "memoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_contrato_memoria_plano_id",
                schema: "pagamento",
                table: "contrato_memoria",
                column: "plano_id");

            migrationBuilder.CreateIndex(
                name: "IX_plano_limites_plano_id_propriedade",
                schema: "pagamento",
                table: "plano_limites",
                columns: new[] { "plano_id", "propriedade" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_plano_recursos_plano_id_ativo_ordem",
                schema: "pagamento",
                table: "plano_recursos",
                columns: new[] { "plano_id", "ativo", "ordem" });

            migrationBuilder.CreateIndex(
                name: "IX_plano_recursos_plano_id_recurso_key",
                schema: "pagamento",
                table: "plano_recursos",
                columns: new[] { "plano_id", "recurso_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_planos_code",
                schema: "pagamento",
                table: "planos",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_planos_tema_id_ativo",
                schema: "pagamento",
                table: "planos",
                columns: new[] { "tema_id", "ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_planos_tipo_pagamento_id",
                schema: "pagamento",
                table: "planos",
                column: "tipo_pagamento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contrato_historico",
                schema: "pagamento");

            migrationBuilder.DropTable(
                name: "plano_limites",
                schema: "pagamento");

            migrationBuilder.DropTable(
                name: "plano_recursos",
                schema: "pagamento");

            migrationBuilder.DropTable(
                name: "contrato_memoria",
                schema: "pagamento");

            migrationBuilder.DropTable(
                name: "contrato_origem",
                schema: "pagamento");

            migrationBuilder.DropTable(
                name: "contrato_status",
                schema: "pagamento");

            migrationBuilder.DropTable(
                name: "planos",
                schema: "pagamento");

            migrationBuilder.DropTable(
                name: "tipo_pagamento",
                schema: "pagamento");
        }
    }
}
