using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WorkSessionsTableNewField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_driver_work_session",
                table: "driver_work_session");

            migrationBuilder.AlterColumn<DateTime>(
                name: "work_start_at",
                table: "driver_work_session",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "route_id",
                table: "driver_work_session",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_driver_work_session",
                table: "driver_work_session",
                columns: new[] { "driver_id", "work_start_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_driver_work_session",
                table: "driver_work_session");

            migrationBuilder.DropColumn(
                name: "route_id",
                table: "driver_work_session");

            migrationBuilder.AlterColumn<DateTime>(
                name: "work_start_at",
                table: "driver_work_session",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "pk_driver_work_session",
                table: "driver_work_session",
                column: "driver_id");
        }
    }
}
