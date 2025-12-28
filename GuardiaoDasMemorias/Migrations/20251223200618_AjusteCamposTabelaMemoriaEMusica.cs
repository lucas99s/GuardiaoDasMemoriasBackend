using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuardiaoDasMemorias.Migrations
{
    /// <inheritdoc />
    public partial class AjusteCamposTabelaMemoriaEMusica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_musicas_clientes_cliente_id",
                schema: "musica",
                table: "musicas");

            migrationBuilder.RenameColumn(
                name: "cliente_id",
                schema: "musica",
                table: "musicas",
                newName: "memoria_id");

            migrationBuilder.RenameIndex(
                name: "IX_musicas_cliente_id",
                schema: "musica",
                table: "musicas",
                newName: "IX_musicas_memoria_id");

            migrationBuilder.AddColumn<string>(
                name: "memoria_hash",
                schema: "memoria",
                table: "memorias",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_musicas_memorias_memoria_id",
                schema: "musica",
                table: "musicas",
                column: "memoria_id",
                principalSchema: "memoria",
                principalTable: "memorias",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_musicas_memorias_memoria_id",
                schema: "musica",
                table: "musicas");

            migrationBuilder.DropColumn(
                name: "memoria_hash",
                schema: "memoria",
                table: "memorias");

            migrationBuilder.RenameColumn(
                name: "memoria_id",
                schema: "musica",
                table: "musicas",
                newName: "cliente_id");

            migrationBuilder.RenameIndex(
                name: "IX_musicas_memoria_id",
                schema: "musica",
                table: "musicas",
                newName: "IX_musicas_cliente_id");

            migrationBuilder.AddForeignKey(
                name: "FK_musicas_clientes_cliente_id",
                schema: "musica",
                table: "musicas",
                column: "cliente_id",
                principalSchema: "cliente",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
