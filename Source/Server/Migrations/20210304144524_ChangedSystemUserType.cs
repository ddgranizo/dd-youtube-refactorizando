using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Refactorizando.Server.Migrations
{
    public partial class ChangedSystemUserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId1",
                table: "LikeRequests");

            migrationBuilder.DropIndex(
                name: "IX_LikeRequests_SystemUserId1",
                table: "LikeRequests");

            migrationBuilder.DropColumn(
                name: "SystemUserId1",
                table: "LikeRequests");

            migrationBuilder.AlterColumn<string>(
                name: "SystemUserId",
                table: "LikeRequests",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "085367c9-4371-42e0-8763-ad2b48cbb47d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "e8b8f272-93a9-45eb-aa78-7b98efc379e4");

            migrationBuilder.CreateIndex(
                name: "IX_LikeRequests_SystemUserId",
                table: "LikeRequests",
                column: "SystemUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId",
                table: "LikeRequests",
                column: "SystemUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId",
                table: "LikeRequests");

            migrationBuilder.DropIndex(
                name: "IX_LikeRequests_SystemUserId",
                table: "LikeRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "SystemUserId",
                table: "LikeRequests",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemUserId1",
                table: "LikeRequests",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "4cd247a0-99f8-4732-962d-23438cfe0e27");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "d91e6be4-07c5-4991-ae2b-567b0be5f371");

            migrationBuilder.CreateIndex(
                name: "IX_LikeRequests_SystemUserId1",
                table: "LikeRequests",
                column: "SystemUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId1",
                table: "LikeRequests",
                column: "SystemUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
