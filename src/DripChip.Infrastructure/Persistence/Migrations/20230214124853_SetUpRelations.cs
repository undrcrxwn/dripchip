using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DripChip.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SetUpRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalTypes_Animals_AnimalId",
                table: "AnimalTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationPoints_Animals_AnimalId",
                table: "LocationPoints");

            migrationBuilder.DropIndex(
                name: "IX_LocationPoints_AnimalId",
                table: "LocationPoints");

            migrationBuilder.DropIndex(
                name: "IX_AnimalTypes_AnimalId",
                table: "AnimalTypes");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "LocationPoints");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "AnimalTypes");

            migrationBuilder.CreateTable(
                name: "AnimalAnimalType",
                columns: table => new
                {
                    AnimalTypesId = table.Column<long>(type: "bigint", nullable: false),
                    AnimalsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalAnimalType", x => new { x.AnimalTypesId, x.AnimalsId });
                    table.ForeignKey(
                        name: "FK_AnimalAnimalType_AnimalTypes_AnimalTypesId",
                        column: x => x.AnimalTypesId,
                        principalTable: "AnimalTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalAnimalType_Animals_AnimalsId",
                        column: x => x.AnimalsId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalLocationPoint",
                columns: table => new
                {
                    VisitedLocationsId = table.Column<long>(type: "bigint", nullable: false),
                    VisitorsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalLocationPoint", x => new { x.VisitedLocationsId, x.VisitorsId });
                    table.ForeignKey(
                        name: "FK_AnimalLocationPoint_Animals_VisitorsId",
                        column: x => x.VisitorsId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalLocationPoint_LocationPoints_VisitedLocationsId",
                        column: x => x.VisitedLocationsId,
                        principalTable: "LocationPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAnimalType_AnimalsId",
                table: "AnimalAnimalType",
                column: "AnimalsId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLocationPoint_VisitorsId",
                table: "AnimalLocationPoint",
                column: "VisitorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalAnimalType");

            migrationBuilder.DropTable(
                name: "AnimalLocationPoint");

            migrationBuilder.AddColumn<long>(
                name: "AnimalId",
                table: "LocationPoints",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AnimalId",
                table: "AnimalTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_AnimalId",
                table: "LocationPoints",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTypes_AnimalId",
                table: "AnimalTypes",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalTypes_Animals_AnimalId",
                table: "AnimalTypes",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationPoints_Animals_AnimalId",
                table: "LocationPoints",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id");
        }
    }
}
