using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace managementapp.Migrations
{
    public partial class datacontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Todolists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Todolists",
                columns: new[] { "Id", "CreatedAt", "Description", "IsCompleted", "Title", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2024, 6, 1, 14, 8, 38, 404, DateTimeKind.Local).AddTicks(1298), "This is the first todo", false, "First Todo", new DateTime(2024, 6, 1, 14, 8, 38, 404, DateTimeKind.Local).AddTicks(1313), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Todolists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Todolists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
