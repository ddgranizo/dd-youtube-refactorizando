using Microsoft.EntityFrameworkCore.Migrations;

namespace Refactorizando.Server.Migrations
{
    public partial class AddedUniqueKeyToRequestLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId",
                table: "LikeRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikeRequests",
                table: "LikeRequests");

            migrationBuilder.DropIndex(
                name: "IX_LikeRequests_RequestId",
                table: "LikeRequests");

            migrationBuilder.AlterColumn<string>(
                name: "SystemUserId",
                table: "LikeRequests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikeRequests",
                table: "LikeRequests",
                columns: new[] { "RequestId", "SystemUserId" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "05228e26-86f4-4731-ba49-d5bce39e87ce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "e5760d39-2de1-46f8-839f-d928e2a45859");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId",
                table: "LikeRequests",
                column: "SystemUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId",
                table: "LikeRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikeRequests",
                table: "LikeRequests");

            migrationBuilder.AlterColumn<string>(
                name: "SystemUserId",
                table: "LikeRequests",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikeRequests",
                table: "LikeRequests",
                column: "Id");

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
                name: "IX_LikeRequests_RequestId",
                table: "LikeRequests",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId",
                table: "LikeRequests",
                column: "SystemUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
