using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Refactorizando.Server.Migrations
{
    public partial class AddedCreatedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Requests",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "fd98bca1-1d8e-4896-8750-f6844a341302");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "6c7b5e21-5784-4a8b-8cf7-fd0f8b5b5c00");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Requests");

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
        }
    }
}
