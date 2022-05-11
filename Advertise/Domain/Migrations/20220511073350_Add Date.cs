using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    public partial class AddDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreate",
                table: "tblAdvertise",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreate",
                table: "tblAdvertise",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
