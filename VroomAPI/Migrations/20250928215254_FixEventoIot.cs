using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VroomAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixEventoIot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_eventos_tags_IdTag",
                table: "eventos");

            migrationBuilder.DropIndex(
                name: "IX_eventos_IdTag",
                table: "eventos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_eventos_IdTag",
                table: "eventos",
                column: "IdTag");

            migrationBuilder.AddForeignKey(
                name: "FK_eventos_tags_IdTag",
                table: "eventos",
                column: "IdTag",
                principalTable: "tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
