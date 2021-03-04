using Microsoft.EntityFrameworkCore.Migrations;

namespace Refactorizando.Server.Migrations
{
    public partial class AddedRequestsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_AspNetUsers_SystemUserId",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Request",
                table: "Request");

            migrationBuilder.RenameTable(
                name: "Request",
                newName: "Requests");

            migrationBuilder.RenameIndex(
                name: "IX_Request_SystemUserId",
                table: "Requests",
                newName: "IX_Requests_SystemUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "5dc9cda2-c4e0-4f45-b90b-029e49ccd499");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "e91f6edf-6e93-4179-8d7a-1e672a87d43a");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_SystemUserId",
                table: "Requests",
                column: "SystemUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_SystemUserId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.RenameTable(
                name: "Requests",
                newName: "Request");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_SystemUserId",
                table: "Request",
                newName: "IX_Request_SystemUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Request",
                table: "Request",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "4939fc70-617e-4687-9534-aee5015ce48b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "9898000b-59e1-49b9-bd16-5d14476fe912");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_AspNetUsers_SystemUserId",
                table: "Request",
                column: "SystemUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
