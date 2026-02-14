using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuardiaoDasMemorias.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailDeClienteEAddComUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_memorias_clientes_cliente_id",
                schema: "memoria",
                table: "memorias");

            migrationBuilder.DropForeignKey(
                name: "FK_musicas_memorias_memoria_id",
                schema: "musica",
                table: "musicas");

            migrationBuilder.DropColumn(
                name: "email",
                schema: "cliente",
                table: "clientes");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                schema: "cliente",
                table: "clientes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_memorias_memoria_hash",
                schema: "memoria",
                table: "memorias",
                column: "memoria_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clientes_user_id",
                schema: "cliente",
                table: "clientes",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_clientes_users_user_id",
                schema: "cliente",
                table: "clientes",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_memorias_clientes_cliente_id",
                schema: "memoria",
                table: "memorias",
                column: "cliente_id",
                principalSchema: "cliente",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_musicas_memorias_memoria_id",
                schema: "musica",
                table: "musicas",
                column: "memoria_id",
                principalSchema: "memoria",
                principalTable: "memorias",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientes_users_user_id",
                schema: "cliente",
                table: "clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_memorias_clientes_cliente_id",
                schema: "memoria",
                table: "memorias");

            migrationBuilder.DropForeignKey(
                name: "FK_musicas_memorias_memoria_id",
                schema: "musica",
                table: "musicas");

            migrationBuilder.DropIndex(
                name: "IX_memorias_memoria_hash",
                schema: "memoria",
                table: "memorias");

            migrationBuilder.DropIndex(
                name: "IX_clientes_user_id",
                schema: "cliente",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "cliente",
                table: "clientes");

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "cliente",
                table: "clientes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_memorias_clientes_cliente_id",
                schema: "memoria",
                table: "memorias",
                column: "cliente_id",
                principalSchema: "cliente",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
