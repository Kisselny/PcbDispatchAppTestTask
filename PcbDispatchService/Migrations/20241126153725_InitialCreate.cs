using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PcbDispatchService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentTypes",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    AvailableSupply = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentTypes", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "PrintedCircuitBoards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    BusinessProcessStatus = table.Column<int>(type: "integer", nullable: false),
                    QualityControlStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintedCircuitBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoardComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentType = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PrintedCircuitBoardId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardComponents_PrintedCircuitBoards_PrintedCircuitBoardId",
                        column: x => x.PrintedCircuitBoardId,
                        principalTable: "PrintedCircuitBoards",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ComponentTypes",
                columns: new[] { "Name", "AvailableSupply" },
                values: new object[,]
                {
                    { "A0-B0", 28 },
                    { "A0-B1", 66 },
                    { "A1-B0", 47 },
                    { "A1-B1", 12 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardComponents_PrintedCircuitBoardId",
                table: "BoardComponents",
                column: "PrintedCircuitBoardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardComponents");

            migrationBuilder.DropTable(
                name: "ComponentTypes");

            migrationBuilder.DropTable(
                name: "PrintedCircuitBoards");
        }
    }
}
