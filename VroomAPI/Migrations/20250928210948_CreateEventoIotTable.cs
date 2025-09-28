using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VroomAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateEventoIotTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Coordenada",
                table: "tags",
                type: "NVARCHAR2(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "motos",
                type: "NVARCHAR2(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoProblema",
                table: "motos",
                type: "NVARCHAR2(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "Chassi",
                table: "motos",
                type: "NVARCHAR2(17)",
                maxLength: 17,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.CreateTable(
                name: "eventos",
                columns: table => new
                {
                    IdTag = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Timestamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    LedOn = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Problema = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Cor = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventos", x => x.IdTag);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eventos");

            migrationBuilder.AlterColumn<string>(
                name: "Coordenada",
                table: "tags",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "motos",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoProblema",
                table: "motos",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Chassi",
                table: "motos",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(17)",
                oldMaxLength: 17);
        }
    }
}
