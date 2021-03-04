using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Refactorizando.Server.Migrations
{
    public partial class AddedLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SystemUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SystemUserId1 = table.Column<string>(type: "varchar(255)", nullable: true),
                    RequestId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_SystemUserId1",
                        column: x => x.SystemUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Likes_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Likes_RequestId",
                table: "Likes",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_SystemUserId1",
                table: "Likes",
                column: "SystemUserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "8e52ccba-c6fc-4a4b-ba05-adaa26bfa799");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "7c7c00a3-94f4-4536-9f62-b45f78c4f632");
        }
    }
}
