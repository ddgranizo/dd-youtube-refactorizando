using Microsoft.EntityFrameworkCore.Migrations;

namespace Refactorizando.Server.Migrations
{
    public partial class RemovedIsConfirmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "885774e8-9231-4009-b9fe-f02bcb2d6f0b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "065c9dac-f3f5-466f-92c8-ef98af50f95c");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c51d0e-6c24-4191-970b-aefb93fb9c51",
                column: "ConcurrencyStamp",
                value: "ad2d8f32-0c7c-4957-a033-73310f5dac37");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adce8478-150e-4f7a-8dff-22bf15a0be0f",
                column: "ConcurrencyStamp",
                value: "51308929-5fd5-4f3b-87d3-969e87337248");
        }
    }
}
