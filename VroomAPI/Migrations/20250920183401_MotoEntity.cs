using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VroomAPI.Migrations
{
    /// <inheritdoc />
    public partial class MotoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "motos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Placa = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Chassi = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DescricaoProblema = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ModeloMoto = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CategoriaProblema = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TagId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_motos_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_motos_TagId",
                table: "motos",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "motos");
        }
    }
}
