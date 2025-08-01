using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WorkSessionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "driver_work_session",
                columns: table => new
                {
                    driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    work_start_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    work_end_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_work_session", x => x.driver_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driver_work_session");
        }
    }
}
