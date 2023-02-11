using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DripChip.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SplitAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

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

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Animals",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ChipperId",
                table: "Animals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChippingDateTime",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ChippingLocationId",
                table: "Animals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeathDateTime",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Animals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "Animals",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Length",
                table: "Animals",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "LifeStatus",
                table: "Animals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Animals",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_AnimalId",
                table: "LocationPoints",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTypes_AnimalId",
                table: "AnimalTypes",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ChipperId",
                table: "Animals",
                column: "ChipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ChippingLocationId",
                table: "Animals",
                column: "ChippingLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Accounts_ChipperId",
                table: "Animals",
                column: "ChipperId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_LocationPoints_ChippingLocationId",
                table: "Animals",
                column: "ChippingLocationId",
                principalTable: "LocationPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Accounts_ChipperId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_LocationPoints_ChippingLocationId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalTypes_Animals_AnimalId",
                table: "AnimalTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationPoints_Animals_AnimalId",
                table: "LocationPoints");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_LocationPoints_AnimalId",
                table: "LocationPoints");

            migrationBuilder.DropIndex(
                name: "IX_AnimalTypes_AnimalId",
                table: "AnimalTypes");

            migrationBuilder.DropIndex(
                name: "IX_Animals_ChipperId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_ChippingLocationId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "LocationPoints");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "AnimalTypes");

            migrationBuilder.DropColumn(
                name: "ChipperId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "ChippingDateTime",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "ChippingLocationId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "DeathDateTime",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "LifeStatus",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Animals");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Animals",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
