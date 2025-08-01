using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DelayTableCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cargos_address_delivery_address_id",
                table: "cargos");

            migrationBuilder.DropForeignKey(
                name: "fk_cargos_address_pick_up_address_id",
                table: "cargos");

            migrationBuilder.DropForeignKey(
                name: "fk_issue_proofs_issues_issue_id",
                table: "issue_proofs");

            migrationBuilder.DropForeignKey(
                name: "fk_routes_cargos_cargo_id",
                table: "routes");

            migrationBuilder.DropForeignKey(
                name: "fk_routes_drivers_driver_id",
                table: "routes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_routes",
                table: "routes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_issues",
                table: "issues");

            migrationBuilder.DropPrimaryKey(
                name: "pk_issue_proofs",
                table: "issue_proofs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_drivers_chats",
                table: "drivers_chats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_drivers",
                table: "drivers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cargos",
                table: "cargos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_address",
                table: "address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "routes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "routes",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "route_status",
                table: "routes",
                newName: "RouteStatus");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                table: "routes",
                newName: "DriverId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "routes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cargo_id",
                table: "routes",
                newName: "CargoId");

            migrationBuilder.RenameIndex(
                name: "ix_routes_driver_id",
                table: "routes",
                newName: "IX_routes_DriverId");

            migrationBuilder.RenameIndex(
                name: "ix_routes_cargo_id",
                table: "routes",
                newName: "IX_routes_CargoId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "issues",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "issue_type",
                table: "issues",
                newName: "IssueType");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                table: "issues",
                newName: "DriverId");

            migrationBuilder.RenameColumn(
                name: "dispatcher_id",
                table: "issues",
                newName: "DispatcherId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "issues",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "chat_id",
                table: "issues",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "ix_issues_dispatcher_id",
                table: "issues",
                newName: "IX_issues_DispatcherId");

            migrationBuilder.RenameIndex(
                name: "ix_issues_chat_id",
                table: "issues",
                newName: "IX_issues_ChatId");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "issue_proofs",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "issue_proofs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "message_type",
                table: "issue_proofs",
                newName: "MessageType");

            migrationBuilder.RenameColumn(
                name: "issue_id",
                table: "issue_proofs",
                newName: "IssueId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "issue_proofs",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_issue_proofs_issue_id",
                table: "issue_proofs",
                newName: "IX_issue_proofs_IssueId");

            migrationBuilder.RenameColumn(
                name: "driver_phone",
                table: "drivers_chats",
                newName: "DriverPhone");

            migrationBuilder.RenameColumn(
                name: "chat_id",
                table: "drivers_chats",
                newName: "ChatId");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                table: "drivers_chats",
                newName: "DriverId");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "drivers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "drivers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "drivers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "drivers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "drivers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "drivers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "drivers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "cargos",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "cargos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "cargos",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "reference_number",
                table: "cargos",
                newName: "ReferenceNumber");

            migrationBuilder.RenameColumn(
                name: "planned_pick_up_date_time",
                table: "cargos",
                newName: "PlannedPickUpDateTime");

            migrationBuilder.RenameColumn(
                name: "planned_delivery_date_time",
                table: "cargos",
                newName: "PlannedDeliveryDateTime");

            migrationBuilder.RenameColumn(
                name: "pick_up_date_time",
                table: "cargos",
                newName: "PickUpDateTime");

            migrationBuilder.RenameColumn(
                name: "pick_up_address_id",
                table: "cargos",
                newName: "PickUpAddressId");

            migrationBuilder.RenameColumn(
                name: "number_of_pallets",
                table: "cargos",
                newName: "NumberOfPallets");

            migrationBuilder.RenameColumn(
                name: "delivery_date_time",
                table: "cargos",
                newName: "DeliveryDateTime");

            migrationBuilder.RenameColumn(
                name: "delivery_address_id",
                table: "cargos",
                newName: "DeliveryAddressId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "cargos",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_cargos_reference_number",
                table: "cargos",
                newName: "IX_cargos_ReferenceNumber");

            migrationBuilder.RenameIndex(
                name: "ix_cargos_pick_up_address_id",
                table: "cargos",
                newName: "IX_cargos_PickUpAddressId");

            migrationBuilder.RenameIndex(
                name: "ix_cargos_delivery_address_id",
                table: "cargos",
                newName: "IX_cargos_DeliveryAddressId");

            migrationBuilder.RenameColumn(
                name: "street",
                table: "address",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "address",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "address",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "address",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "address",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "post_code",
                table: "address",
                newName: "PostCode");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "address",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "country_code",
                table: "address",
                newName: "CountryCode");

            migrationBuilder.RenameColumn(
                name: "contact_person_phone_number",
                table: "address",
                newName: "ContactPersonPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "contact_person_last_name",
                table: "address",
                newName: "ContactPersonLastName");

            migrationBuilder.RenameColumn(
                name: "contact_person_first_name",
                table: "address",
                newName: "ContactPersonFirstName");

            migrationBuilder.RenameColumn(
                name: "company_name",
                table: "address",
                newName: "CompanyName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_routes",
                table: "routes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_issues",
                table: "issues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_issue_proofs",
                table: "issue_proofs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drivers_chats",
                table: "drivers_chats",
                column: "DriverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drivers",
                table: "drivers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cargos",
                table: "cargos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_address",
                table: "address",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "delay_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    DispatcherId = table.Column<Guid>(type: "uuid", nullable: false),
                    DelayTime = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delay_requests", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_cargos_address_DeliveryAddressId",
                table: "cargos",
                column: "DeliveryAddressId",
                principalTable: "address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cargos_address_PickUpAddressId",
                table: "cargos",
                column: "PickUpAddressId",
                principalTable: "address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_issue_proofs_issues_IssueId",
                table: "issue_proofs",
                column: "IssueId",
                principalTable: "issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_routes_cargos_CargoId",
                table: "routes",
                column: "CargoId",
                principalTable: "cargos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_routes_drivers_DriverId",
                table: "routes",
                column: "DriverId",
                principalTable: "drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cargos_address_DeliveryAddressId",
                table: "cargos");

            migrationBuilder.DropForeignKey(
                name: "FK_cargos_address_PickUpAddressId",
                table: "cargos");

            migrationBuilder.DropForeignKey(
                name: "FK_issue_proofs_issues_IssueId",
                table: "issue_proofs");

            migrationBuilder.DropForeignKey(
                name: "FK_routes_cargos_CargoId",
                table: "routes");

            migrationBuilder.DropForeignKey(
                name: "FK_routes_drivers_DriverId",
                table: "routes");

            migrationBuilder.DropTable(
                name: "delay_requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_routes",
                table: "routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_issues",
                table: "issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_issue_proofs",
                table: "issue_proofs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_drivers_chats",
                table: "drivers_chats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_drivers",
                table: "drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cargos",
                table: "cargos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_address",
                table: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "routes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "routes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RouteStatus",
                table: "routes",
                newName: "route_status");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "routes",
                newName: "driver_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "routes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CargoId",
                table: "routes",
                newName: "cargo_id");

            migrationBuilder.RenameIndex(
                name: "IX_routes_DriverId",
                table: "routes",
                newName: "ix_routes_driver_id");

            migrationBuilder.RenameIndex(
                name: "IX_routes_CargoId",
                table: "routes",
                newName: "ix_routes_cargo_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "issues",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IssueType",
                table: "issues",
                newName: "issue_type");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "issues",
                newName: "driver_id");

            migrationBuilder.RenameColumn(
                name: "DispatcherId",
                table: "issues",
                newName: "dispatcher_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "issues",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "issues",
                newName: "chat_id");

            migrationBuilder.RenameIndex(
                name: "IX_issues_DispatcherId",
                table: "issues",
                newName: "ix_issues_dispatcher_id");

            migrationBuilder.RenameIndex(
                name: "IX_issues_ChatId",
                table: "issues",
                newName: "ix_issues_chat_id");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "issue_proofs",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "issue_proofs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MessageType",
                table: "issue_proofs",
                newName: "message_type");

            migrationBuilder.RenameColumn(
                name: "IssueId",
                table: "issue_proofs",
                newName: "issue_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "issue_proofs",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_issue_proofs_IssueId",
                table: "issue_proofs",
                newName: "ix_issue_proofs_issue_id");

            migrationBuilder.RenameColumn(
                name: "DriverPhone",
                table: "drivers_chats",
                newName: "driver_phone");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "drivers_chats",
                newName: "chat_id");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "drivers_chats",
                newName: "driver_id");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "drivers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "drivers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "drivers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "drivers",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "drivers",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "drivers",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "drivers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "cargos",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "cargos",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "cargos",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ReferenceNumber",
                table: "cargos",
                newName: "reference_number");

            migrationBuilder.RenameColumn(
                name: "PlannedPickUpDateTime",
                table: "cargos",
                newName: "planned_pick_up_date_time");

            migrationBuilder.RenameColumn(
                name: "PlannedDeliveryDateTime",
                table: "cargos",
                newName: "planned_delivery_date_time");

            migrationBuilder.RenameColumn(
                name: "PickUpDateTime",
                table: "cargos",
                newName: "pick_up_date_time");

            migrationBuilder.RenameColumn(
                name: "PickUpAddressId",
                table: "cargos",
                newName: "pick_up_address_id");

            migrationBuilder.RenameColumn(
                name: "NumberOfPallets",
                table: "cargos",
                newName: "number_of_pallets");

            migrationBuilder.RenameColumn(
                name: "DeliveryDateTime",
                table: "cargos",
                newName: "delivery_date_time");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddressId",
                table: "cargos",
                newName: "delivery_address_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "cargos",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_cargos_ReferenceNumber",
                table: "cargos",
                newName: "ix_cargos_reference_number");

            migrationBuilder.RenameIndex(
                name: "IX_cargos_PickUpAddressId",
                table: "cargos",
                newName: "ix_cargos_pick_up_address_id");

            migrationBuilder.RenameIndex(
                name: "IX_cargos_DeliveryAddressId",
                table: "cargos",
                newName: "ix_cargos_delivery_address_id");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "address",
                newName: "street");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "address",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "address",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "address",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "address",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PostCode",
                table: "address",
                newName: "post_code");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "address",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CountryCode",
                table: "address",
                newName: "country_code");

            migrationBuilder.RenameColumn(
                name: "ContactPersonPhoneNumber",
                table: "address",
                newName: "contact_person_phone_number");

            migrationBuilder.RenameColumn(
                name: "ContactPersonLastName",
                table: "address",
                newName: "contact_person_last_name");

            migrationBuilder.RenameColumn(
                name: "ContactPersonFirstName",
                table: "address",
                newName: "contact_person_first_name");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "address",
                newName: "company_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_routes",
                table: "routes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_issues",
                table: "issues",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_issue_proofs",
                table: "issue_proofs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_drivers_chats",
                table: "drivers_chats",
                column: "driver_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_drivers",
                table: "drivers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cargos",
                table: "cargos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_address",
                table: "address",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_cargos_address_delivery_address_id",
                table: "cargos",
                column: "delivery_address_id",
                principalTable: "address",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_cargos_address_pick_up_address_id",
                table: "cargos",
                column: "pick_up_address_id",
                principalTable: "address",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_issue_proofs_issues_issue_id",
                table: "issue_proofs",
                column: "issue_id",
                principalTable: "issues",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_routes_cargos_cargo_id",
                table: "routes",
                column: "cargo_id",
                principalTable: "cargos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_routes_drivers_driver_id",
                table: "routes",
                column: "driver_id",
                principalTable: "drivers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
