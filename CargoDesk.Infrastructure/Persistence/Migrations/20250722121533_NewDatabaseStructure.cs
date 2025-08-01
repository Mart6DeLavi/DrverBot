using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewDatabaseStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cargos_drivers_driver_id",
                table: "cargos");

            migrationBuilder.DropForeignKey(
                name: "fk_cargos_routes_route_id",
                table: "cargos");

            migrationBuilder.DropIndex(
                name: "ix_cargos_driver_id",
                table: "cargos");

            migrationBuilder.DropIndex(
                name: "ix_cargos_route_id",
                table: "cargos");

            migrationBuilder.DropColumn(
                name: "driver_id",
                table: "cargos");

            migrationBuilder.DropColumn(
                name: "route_id",
                table: "cargos");

            migrationBuilder.AddColumn<Guid>(
                name: "cargo_id",
                table: "routes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "number_of_pallets",
                table: "cargos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "weight",
                table: "cargos",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "address",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "contact_person_phone_number",
                table: "address",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "number_of_pallets",
                table: "cargos");

            migrationBuilder.DropColumn(
                name: "weight",
                table: "cargos");

            migrationBuilder.AddColumn<Guid>(
                name: "driver_id",
                table: "cargos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "route_id",
                table: "cargos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "address",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "contact_person_phone_number",
                table: "address",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_cargos_driver_id",
                table: "cargos",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_cargos_route_id",
                table: "cargos",
                column: "route_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_cargos_drivers_driver_id",
                table: "cargos",
                column: "driver_id",
                principalTable: "drivers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cargos_routes_route_id",
                table: "cargos",
                column: "route_id",
                principalTable: "routes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
