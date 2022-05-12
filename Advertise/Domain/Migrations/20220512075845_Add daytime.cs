using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    public partial class Adddaytime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreate",
                table: "tblAdvertise",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                 name: "DateCreate",
                table: "tblAdvertise",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
        }
    }
}
