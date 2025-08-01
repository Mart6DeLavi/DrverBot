using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameRouteCargoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_cargos_route_id",
                table: "cargos");

            migrationBuilder.CreateIndex(
                name: "ix_cargos_route_id",
                table: "cargos",
                column: "route_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_cargos_route_id",
                table: "cargos");

            migrationBuilder.CreateIndex(
                name: "ix_cargos_route_id",
                table: "cargos",
                column: "route_id",
                unique: true);
        }
    }
}
