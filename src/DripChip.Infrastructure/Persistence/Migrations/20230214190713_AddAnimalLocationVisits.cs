using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DripChip.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimalLocationVisits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalLocationPoint");

            migrationBuilder.CreateTable(
                name: "AnimalLocationVisits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VisitorId = table.Column<long>(type: "bigint", nullable: false),
                    LocationPointId = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeOfVisitLocationPoint = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalLocationVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalLocationVisits_Animals_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalLocationVisits_LocationPoints_LocationPointId",
                        column: x => x.LocationPointId,
                        principalTable: "LocationPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLocationVisits_LocationPointId",
                table: "AnimalLocationVisits",
                column: "LocationPointId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLocationVisits_VisitorId",
                table: "AnimalLocationVisits",
                column: "VisitorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalLocationVisits");

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
                name: "IX_AnimalLocationPoint_VisitorsId",
                table: "AnimalLocationPoint",
                column: "VisitorsId");
        }
    }
}
