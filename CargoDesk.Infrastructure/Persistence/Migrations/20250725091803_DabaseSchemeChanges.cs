using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DabaseSchemeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_routes_cargos_cargo_id",
                table: "routes");

            migrationBuilder.DropIndex(
                name: "ix_routes_cargo_id",
                table: "routes");

            migrationBuilder.DropColumn(
                name: "cargo_id",
                table: "routes");

            migrationBuilder.DropColumn(
                name: "route_status",
                table: "routes");

            migrationBuilder.AddColumn<string>(
                name: "cargo_ids",
                table: "routes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "route_cargo_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    route_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cargo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_route_cargo_statuses", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_route_cargo_statuses_route_id_cargo_id",
                table: "route_cargo_statuses",
                columns: new[] { "route_id", "cargo_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "route_cargo_statuses");

            migrationBuilder.DropColumn(
                name: "cargo_ids",
                table: "routes");

            migrationBuilder.AddColumn<Guid>(
                name: "cargo_id",
                table: "routes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "route_status",
                table: "routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_routes_cargo_id",
                table: "routes",
                column: "cargo_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_routes_cargos_cargo_id",
                table: "routes",
                column: "cargo_id",
                principalTable: "cargos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
