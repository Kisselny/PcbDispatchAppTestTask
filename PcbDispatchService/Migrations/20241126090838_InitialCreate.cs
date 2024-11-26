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
                name: "BusinessProcessStateBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessProcessType = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProcessStateBase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentTypes",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    Name = table.Column<string>(type: "text", nullable: false),
                    BusinessProcessStateBaseId = table.Column<int>(type: "integer", nullable: false),
                    QualityControlStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintedCircuitBoards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrintedCircuitBoards_BusinessProcessStateBase_BusinessProce~",
                        column: x => x.BusinessProcessStateBaseId,
                        principalTable: "BusinessProcessStateBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardComponent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentTypeName = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PrintedCircuitBoardId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardComponent_ComponentTypes_ComponentTypeName",
                        column: x => x.ComponentTypeName,
                        principalTable: "ComponentTypes",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardComponent_PrintedCircuitBoards_PrintedCircuitBoardId",
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
                name: "IX_BoardComponent_ComponentTypeName",
                table: "BoardComponent",
                column: "ComponentTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_BoardComponent_PrintedCircuitBoardId",
                table: "BoardComponent",
                column: "PrintedCircuitBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_PrintedCircuitBoards_BusinessProcessStateBaseId",
                table: "PrintedCircuitBoards",
                column: "BusinessProcessStateBaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardComponent");

            migrationBuilder.DropTable(
                name: "ComponentTypes");

            migrationBuilder.DropTable(
                name: "PrintedCircuitBoards");

            migrationBuilder.DropTable(
                name: "BusinessProcessStateBase");
        }
    }
}
