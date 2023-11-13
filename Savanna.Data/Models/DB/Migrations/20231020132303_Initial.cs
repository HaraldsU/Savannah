using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savanna.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalBase",
                columns: table => new
                {
                    AnimalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstLetter = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    KeyBind = table.Column<int>(type: "int", nullable: false),
                    AnimalType = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    Range = table.Column<int>(type: "int", nullable: false),
                    Health = table.Column<float>(type: "real", nullable: false),
                    BreedingCooldown = table.Column<int>(type: "int", nullable: false),
                    BreedingTime = table.Column<int>(type: "int", nullable: false),
                    ActiveBreedingCooldown = table.Column<int>(type: "int", nullable: false),
                    IsBirthing = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalBase", x => x.AnimalId);
                });

            migrationBuilder.CreateTable(
                name: "GameState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    //.Annotation("SqlServer:Identity", "1, 1"),
                    Turn = table.Column<int>(type: "int", nullable: false),
                    CurrentTypeIndex = table.Column<int>(type: "int", nullable: false),
                    Dimensions = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GridCellModel",
                columns: table => new
                {
                    CellId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: true),
                    GameStateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridCellModel", x => x.CellId);
                    table.ForeignKey(
                        name: "FK_GridCellModel_AnimalBase_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "AnimalBase",
                        principalColumn: "AnimalId");
                    table.ForeignKey(
                        name: "FK_GridCellModel_GameState_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameState",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GridCellModel_AnimalId",
                table: "GridCellModel",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_GridCellModel_GameStateId",
                table: "GridCellModel",
                column: "GameStateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GridCellModel");

            migrationBuilder.DropTable(
                name: "AnimalBase");

            migrationBuilder.DropTable(
                name: "GameState");
        }
    }
}
