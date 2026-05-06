using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiStock.Migrations
{
    /// <inheritdoc />
    public partial class Correcciónlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepuestoId",
                table: "Logs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_RepuestoId",
                table: "Logs",
                column: "RepuestoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Repuestos_RepuestoId",
                table: "Logs",
                column: "RepuestoId",
                principalTable: "Repuestos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Repuestos_RepuestoId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_RepuestoId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "RepuestoId",
                table: "Logs");
        }
    }
}
