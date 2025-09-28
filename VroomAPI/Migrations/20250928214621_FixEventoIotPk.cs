using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VroomAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixEventoIotPk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_eventos",
                table: "eventos");

            migrationBuilder.AlterColumn<int>(
                name: "IdTag",
                table: "eventos",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "eventos",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0)
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_eventos",
                table: "eventos",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_eventos_tags_IdTag",
                table: "eventos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_eventos",
                table: "eventos");

            migrationBuilder.DropIndex(
                name: "IX_eventos_IdTag",
                table: "eventos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "eventos");

            migrationBuilder.AlterColumn<int>(
                name: "IdTag",
                table: "eventos",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_eventos",
                table: "eventos",
                column: "IdTag");
        }
    }
}
