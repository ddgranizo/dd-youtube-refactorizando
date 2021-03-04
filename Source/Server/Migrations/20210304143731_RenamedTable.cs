using Microsoft.EntityFrameworkCore.Migrations;

namespace Refactorizando.Server.Migrations
{
    public partial class RenamedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_SystemUserId1",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Requests_RequestId",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "LikeRequests");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_SystemUserId1",
                table: "LikeRequests",
                newName: "IX_LikeRequests_SystemUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_RequestId",
                table: "LikeRequests",
                newName: "IX_LikeRequests_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikeRequests",
                table: "LikeRequests",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId1",
                table: "LikeRequests",
                column: "SystemUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikeRequests_Requests_RequestId",
                table: "LikeRequests",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeRequests_AspNetUsers_SystemUserId1",
                table: "LikeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeRequests_Requests_RequestId",
                table: "LikeRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikeRequests",
                table: "LikeRequests");

            migrationBuilder.RenameTable(
                name: "LikeRequests",
                newName: "Likes");

            migrationBuilder.RenameIndex(
                name: "IX_LikeRequests_SystemUserId1",
                table: "Likes",
                newName: "IX_Likes_SystemUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_LikeRequests_RequestId",
                table: "Likes",
                newName: "IX_Likes_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "43ef90cc-ff15-4b32-9371-ea1b952d4aa7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "5c692cb3-0088-41f9-9460-9025123a0a71");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_SystemUserId1",
                table: "Likes",
                column: "SystemUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Requests_RequestId",
                table: "Likes",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
