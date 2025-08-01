using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionTableDriverAndTelegramChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_cargos_route_id",
                table: "cargos");

            migrationBuilder.CreateTable(
                name: "drivers_chats",
                columns: table => new
                {
                    driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    driver_phone = table.Column<string>(type: "text", nullable: false),
                    chat_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drivers_chats", x => x.driver_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cargos_route_id",
                table: "cargos",
                column: "route_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drivers_chats");

            migrationBuilder.DropIndex(
                name: "ix_cargos_route_id",
                table: "cargos");

            migrationBuilder.CreateIndex(
                name: "ix_cargos_route_id",
                table: "cargos",
                column: "route_id");
        }
    }
}
