using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GuardiaoDasMemorias.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cliente");

            migrationBuilder.EnsureSchema(
                name: "memoria");

            migrationBuilder.EnsureSchema(
                name: "musica");

            migrationBuilder.EnsureSchema(
                name: "tema");

            migrationBuilder.EnsureSchema(
                name: "template");

            migrationBuilder.CreateTable(
                name: "clientes",
                schema: "cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "temas",
                schema: "tema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "musicas",
                schema: "musica",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    caminho = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    cliente_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musicas", x => x.id);
                    table.ForeignKey(
                        name: "FK_musicas_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "cliente",
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "templates",
                schema: "template",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    tema_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_templates", x => x.id);
                    table.ForeignKey(
                        name: "FK_templates_temas_tema_id",
                        column: x => x.tema_id,
                        principalSchema: "tema",
                        principalTable: "temas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "memorias",
                schema: "memoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    tema_id = table.Column<int>(type: "integer", nullable: false),
                    template_id = table.Column<int>(type: "integer", nullable: false),
                    cliente_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_memorias", x => x.id);
                    table.ForeignKey(
                        name: "FK_memorias_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "cliente",
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_memorias_temas_tema_id",
                        column: x => x.tema_id,
                        principalSchema: "tema",
                        principalTable: "temas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_memorias_templates_template_id",
                        column: x => x.template_id,
                        principalSchema: "template",
                        principalTable: "templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_memorias_cliente_id",
                schema: "memoria",
                table: "memorias",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_memorias_tema_id",
                schema: "memoria",
                table: "memorias",
                column: "tema_id");

            migrationBuilder.CreateIndex(
                name: "IX_memorias_template_id",
                schema: "memoria",
                table: "memorias",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "IX_musicas_cliente_id",
                schema: "musica",
                table: "musicas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_templates_tema_id",
                schema: "template",
                table: "templates",
                column: "tema_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "memorias",
                schema: "memoria");

            migrationBuilder.DropTable(
                name: "musicas",
                schema: "musica");

            migrationBuilder.DropTable(
                name: "templates",
                schema: "template");

            migrationBuilder.DropTable(
                name: "clientes",
                schema: "cliente");

            migrationBuilder.DropTable(
                name: "temas",
                schema: "tema");
        }
    }
}
